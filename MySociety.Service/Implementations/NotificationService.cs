using System.Linq.Expressions;
using System.Threading.Tasks;
using MySociety.Entity.HelperModels;
using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;
using MySociety.Repository.Interfaces;
using MySociety.Service.Common;
using MySociety.Service.Helper;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class NotificationService : INotificationService
{
    private readonly IGenericRepository<Notification> _notificationRepository;
    private readonly IHttpService _httpService;
    private readonly INotificationHubService _notificationHubService;
    private readonly IGenericRepository<NotificationCategory> _notificationCategoryRepository;

    public NotificationService(IGenericRepository<Notification> notificationMessageRepository, IHttpService httpService, INotificationHubService notificationHubService, IGenericRepository<NotificationCategory> notificationCategoryRepository)
    {
        _notificationRepository = notificationMessageRepository;
        _httpService = httpService;
        _notificationHubService = notificationHubService;
        _notificationCategoryRepository = notificationCategoryRepository;
    }

    public async Task<int> GetUnreadCount()
    {
        int userId = await _httpService.LoggedInUserId();
        int count = _notificationRepository.GetCount(n => n.ReceiverId == userId && n.ReadAt == null);
        return count;
    }

    public List<NotificationCategory> GetNotificationCategories()
    {
        List<NotificationCategory> list = _notificationCategoryRepository.GetAll().ToList();
        return list;
    }

    public async Task MarkAsRead(int notificationId)
    {
        Notification? notification = await _notificationRepository.GetByIdAsync(notificationId);

        if (notification != null)
        {
            notification.ReadAt = DateTime.Now;
            await _notificationRepository.UpdateAsync(notification);
        }
    }

    public async Task MarkAllAsRead()
    {
        int userId = await _httpService.LoggedInUserId();
        DbResult<Notification> dbResult = await _notificationRepository.GetRecords(n => n.ReceiverId == userId && n.ReadAt == null);

        if (dbResult.Records.Any())
        {
            foreach (var notification in dbResult.Records)
            {
                notification.ReadAt = DateTime.Now;
            }

            await _notificationRepository.UpdateRangeAsync(dbResult.Records);
        }
    }

    public async Task<NotificationPagination> List(NotificationFilterVM filter)
    {
        int userId = await _httpService.LoggedInUserId();

         Expression<Func<Notification, bool>> readStatus = v => true;
        if (!string.IsNullOrEmpty(filter.ReadStatus) && filter.ReadStatus.ToLower() != "all")
        {
            switch (filter.ReadStatus.ToLower())
            {
                case "unread":
                    readStatus = v => v.ReadAt == null;
                    break;

                case "read":
                    readStatus = v => v.ReadAt != null;
                    break;

            }
        }
        //Filter on the basis of Date Range
        Expression<Func<Notification, bool>> datePredicate = v => true;      //All

        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        DateOnly now = DateOnly.FromDateTime(DateTime.Now);

        if (Enum.TryParse(filter.DateRange, true, out DateFilterType dateFilter) && filter.DateRange != "all")
        {
            switch (dateFilter)
            {
                case DateFilterType.Today:
                    datePredicate = v =>
                        DateOnly.FromDateTime(v.CreatedAt) == today;
                    break;

                case DateFilterType.Last7Days:
                    DateOnly from7 = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));
                    datePredicate = v =>
                        DateOnly.FromDateTime(v.CreatedAt) >= from7 &&
                        DateOnly.FromDateTime(v.CreatedAt) <= now;
                    break;

                case DateFilterType.Last30Days:
                    DateOnly from30 = DateOnly.FromDateTime(DateTime.Now.AddDays(-30));
                    datePredicate = v =>
                        DateOnly.FromDateTime(v.CreatedAt) >= from30 &&
                        DateOnly.FromDateTime(v.CreatedAt) <= now;
                    break;

                case DateFilterType.CurrentMonth:
                    int currentMonth = DateTime.Now.Month;
                    int currentYear = DateTime.Now.Year;
                    datePredicate = v =>
                        DateOnly.FromDateTime(v.CreatedAt).Month == currentMonth &&
                        DateOnly.FromDateTime(v.CreatedAt).Year == currentYear;
                    break;

                case DateFilterType.CustomDate:
                    if (filter.FromDate.HasValue && filter.ToDate.HasValue)
                    {
                        DateOnly from = filter.FromDate.Value;
                        DateOnly to = filter.ToDate.Value;
                        datePredicate = v =>
                            DateOnly.FromDateTime(v.CreatedAt) >= from &&
                            DateOnly.FromDateTime(v.CreatedAt) <= to;
                    }
                    else if (filter.FromDate.HasValue)
                    {
                        DateOnly from = filter.FromDate.Value;
                        datePredicate = v => DateOnly.FromDateTime(v.CreatedAt) >= from;
                    }
                    else if (filter.ToDate.HasValue)
                    {
                        DateOnly to = filter.ToDate.Value;
                        datePredicate = v => DateOnly.FromDateTime(v.CreatedAt) <= to;
                    }
                    break;
            }
        }

        Expression<Func<Notification, bool>> userPredicate = n => n.ReceiverId == userId;

        Expression<Func<Notification, bool>> categoryPredicate = n => filter.Category == -1 || n.TargetEntityId == filter.Category;

        DbResult<Notification> dbResult = await _notificationRepository.GetRecords(
            predicates: new List<Expression<Func<Notification, bool>>>
            {
                userPredicate,
                readStatus,
                datePredicate,
                categoryPredicate
            },
            orderBy: q => q.OrderByDescending(n => n.CreatedAt),
            pageNumber: filter.PageNumber,
            pageSize: filter.PageSize
        );

        NotificationPagination list = new()
        {
            Notifications = dbResult.Records.Select(r => new NotificationVM
            {
                Id = r.Id,
                Message = r.Message,
                Time = r.CreatedAt,
                ActionTable = r.TargetEntity,
                ActionId = r.TargetId,
                ActionUrl = r.TargetUrl,
                ReadAt = r.ReadAt
            }).ToList()
        };

        list.Page.SetPagination(dbResult.TotalRecord, filter.PageSize, filter.PageNumber);

        return list;
    }

    public async Task VisitorNotification(Notification notify)
    {
        notify.Id = 0;
        notify.TargetEntity = TargetEntity.Visitor.ToString();
        notify.TargetUrl = CommonUrls.VisitorApproval.Replace("{0}", notify.TargetId.ToString());

        await _notificationRepository.AddAsync(notify);
        await _notificationHubService.NotifyUser(notify.ReceiverId, notify.Message);
    }

    public async Task<List<NotificationVM>> List()
    {
        int userId = await _httpService.LoggedInUserId();

        DbResult<Notification> dbResult = await _notificationRepository.GetRecords(
            predicate: m => m.ReceiverId == userId && m.ReadAt == null,
            orderBy: q => q.OrderByDescending(m => m.CreatedAt)
        );

        List<NotificationVM> list = dbResult.Records.Select(r => new NotificationVM
        {
            Id = r.Id,
            Message = r.Message,
            Time = r.CreatedAt,
            ActionTable = r.TargetEntity,
            ActionId = r.TargetId,
            ActionUrl = r.TargetUrl,
        }).ToList();

        return list;
    }

}
