using System.Linq.Expressions;
using System.Threading.Tasks;
using MySociety.Entity;
using MySociety.Entity.HelperModels;
using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;
using MySociety.Repository.Interfaces;
using MySociety.Service.Common;
using MySociety.Service.Exceptions;
using MySociety.Service.Helper;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class NoticeService : INoticeService
{
    private readonly IGenericRepository<Notice> _noticeRepository;
    private readonly IHttpService _httpService;
    private readonly IGenericRepository<NoticeCategory> _noticeCategoryRepository;
    private readonly IGenericRepository<AudienceGroupType> _noticeTargetAudienceRepository;

    public NoticeService(IGenericRepository<Notice> noticeRepository, IHttpService httpService, IGenericRepository<NoticeCategory> noticeCategoryRepository, IGenericRepository<AudienceGroupType> noticeTargetAudienceRepository)
    {
        _noticeRepository = noticeRepository;
        _httpService = httpService;
        _noticeCategoryRepository = noticeCategoryRepository;
        _noticeTargetAudienceRepository = noticeTargetAudienceRepository;

    }

    public async Task<List<NoticeCategory>> GetNoticeCategories()
    {
        DbResult<NoticeCategory> dbRecords = await _noticeCategoryRepository.GetRecords(
            orderBy: q => q.OrderBy(n => n.Id)
        );

        return dbRecords.Records.ToList();
    }

    public List<AudienceGroupType> GetNoticeAudiences()
    {
        return _noticeTargetAudienceRepository.GetAll().ToList();
    }

    public async Task<NoticeVM> Get(int id)
    {
        NoticeVM noticeVM = new();

        Notice? notice = await _noticeRepository.GetByIdAsync(id);
        if (notice == null)
        {
            return noticeVM;
        }

        noticeVM.Id = notice.Id;
        noticeVM.Title = notice.Title;
        noticeVM.Description = notice.Description;
        noticeVM.CategoryId = notice.NoticeCategoryId;

        return noticeVM;
    }

    public async Task<ResponseVM> Save(NoticeVM noticeVM)
    {
        ResponseVM response = new();

        Notice notice = await _noticeRepository.GetByIdAsync(noticeVM.Id) ?? new();

        notice.Title = noticeVM.Title;
        notice.Description = noticeVM.Description;
        notice.NoticeCategoryId = noticeVM.CategoryId;
        notice.UpdatedAt = DateTime.Now;
        notice.UpdatedBy = await _httpService.LoggedInUserId();

        if (notice.Id == 0)             //Add Notice
        {
            notice.CreatedAt = DateTime.Now;
            notice.CreatedBy = await _httpService.LoggedInUserId();

            await _noticeRepository.AddAsync(notice);
            response.Message = NotificationMessages.Added.Replace("{0}", "Notice");
        }
        else                            //Update Notice
        {
            await _noticeRepository.UpdateAsync(notice);
            response.Message = NotificationMessages.Updated.Replace("{0}", "Notice");
        }

        response.Success = true;
        return response;
    }
    
    

    public async Task<NoticePagination> List(NoticeFilterVM filter)
    {
        filter.Search = string.IsNullOrEmpty(filter.Search) ? "" : filter.Search.Replace(" ", "");

        //For sorting the column according to order
        Func<IQueryable<Notice>, IOrderedQueryable<Notice>>? orderBy = q => q.OrderBy(v => v.Id);

        if (!string.IsNullOrEmpty(filter.Column))
        {
            switch (filter.Column.ToLower())
            {
                case "title":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(v => v.Title) : q => q.OrderByDescending(v => v.Title);
                    break;
                case "createdat":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(v => v.CreatedAt) : q => q.OrderByDescending(v => v.CreatedAt);
                    break;
                case "updatedat":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(v => v.UpdatedAt) : q => q.OrderByDescending(v => v.UpdatedAt);
                    break;
                default:
                    break;
            }
        }

        //Filter on the basis of Date Range
        Expression<Func<Notice, bool>> datePredicate = v => true;      //All

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

        Expression<Func<Notice, bool>> predicate = n => n.DeletedBy == null &&
                            (filter.CategoryId == -1 || n.NoticeCategoryId == filter.CategoryId) &&
                            (string.IsNullOrEmpty(filter.Search.ToLower()) ||
                            n.Title.ToLower().Contains(filter.Search.ToLower()));

        DbResult<Notice> dbResult = await _noticeRepository.GetRecords(
            predicates: new List<Expression<Func<Notice, bool>>>
            {
                datePredicate,
                predicate
            },
            orderBy: orderBy,
            pageSize: filter.PageSize,
            pageNumber: filter.PageNumber
        );

        NoticePagination vehicles = new()
        {
            Notices = dbResult.Records.Select(r => new NoticeVM
            {
                Id = r.Id,
                Title = r.Title,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            }).ToList()
        };

        vehicles.Page.SetPagination(dbResult.TotalRecord, filter.PageSize, filter.PageNumber);

        return vehicles;
    }

    public async Task Delete(int id)
    {
        Notice notice = await _noticeRepository.GetByIdAsync(id)
                    ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "vehicle"));

        notice.DeletedBy = await _httpService.LoggedInUserId();
        notice.DeletedAt = DateTime.Now;

        await _noticeRepository.UpdateAsync(notice);
    }

}
