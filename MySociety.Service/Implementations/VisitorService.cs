using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySociety.Entity.HelperModels;
using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;
using MySociety.Repository.Interfaces;
using MySociety.Service.Common;
using MySociety.Service.Exceptions;
using MySociety.Service.Helper;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class VisitorService : IVisitorService
{
    private readonly IGenericRepository<Visitor> _visitorRepository;
    private readonly IGenericRepository<VisitPurpose> _visitPurposeRepository;
    private readonly IBlockService _blockService;
    private readonly IHouseMappingService _houseMappingService;
    private readonly IHttpService _httpService;
    private readonly IVisitorFeedbackService _feedbackService;
    private readonly IUserService _userService;
    private readonly IRoleService _roleService;
    private readonly INotificationService _notificationService;
    private readonly IGenericRepository<VisitorStatus> _visitorStatusRepository;

    public VisitorService(IGenericRepository<Visitor> visitorRepository, IGenericRepository<VisitPurpose> visitPurposeRepository, IBlockService blockService, IHouseMappingService houseMappingService, IHttpService httpService, IVisitorFeedbackService feedbackService, IUserService userService, IRoleService roleService, INotificationService notificationService, IGenericRepository<VisitorStatus> visitorStatusRepository)
    {
        _visitorRepository = visitorRepository;
        _visitPurposeRepository = visitPurposeRepository;
        _blockService = blockService;
        _houseMappingService = houseMappingService;
        _httpService = httpService;
        _feedbackService = feedbackService;
        _userService = userService;
        _roleService = roleService;
        _notificationService = notificationService;
        _visitorStatusRepository = visitorStatusRepository;

    }

    public async Task<VisitorVM> Get(int id)
    {
        VisitorVM visitorVM = new()
        {
            VisitPurposes = _visitPurposeRepository.GetAll().ToList(),
            Address = new()
            {
                Blocks = await _blockService.List()
            }
        };

        Visitor? visitor = await _visitorRepository.GetByStringAsync(
                            predicate: v => v.Id == id,
                            queries: new List<Func<IQueryable<Visitor>, IQueryable<Visitor>>>
                            {
                                q => q.Include(v => v.VisitPurpose),
                                q => q.Include(v => v.VisitorFeedbacks),
                                q => q.Include(v => v.Status)
                            });

        if (visitor == null)
        {
            return visitorVM;
        }

        visitorVM.Id = visitor.Id;
        visitorVM.Name = visitor.Name;
        visitorVM.Phone = visitor.Phone;
        visitorVM.VisitPurposeId = visitor.VisitPurposeId;
        visitorVM.VisitReason = visitor.VisitPurposeReason;
        visitorVM.NoOfVisitors = visitor.NoOfVisitors;
        visitorVM.VehicleNumber = visitor.VehicleNo;
        visitorVM.ApprovalStatus = visitor.Status.Name;
        visitorVM.CreatedAt = visitor.CreatedAt;
        visitorVM.CheckIn = visitor.CheckInTime;
        visitorVM.CheckOut = visitor.CheckOutTime;
        visitorVM.Address = await _houseMappingService.GetAddress(visitor.HouseMappingId);
        visitorVM.Address.Blocks = await _blockService.List();
        visitorVM.VisitPurpose = visitor.VisitPurpose.Name == "Other" ? visitor.VisitPurposeReason : visitor.VisitPurpose.Name;

        // if (visitor.IsApproved == null && (DateTime.Now - visitor.CreatedAt).TotalMinutes <= 30)
        // {
        //     visitorVM.ApprovalStatus = ApprovalStatus.Pending.ToString();
        // }
        // else if (visitor.IsApproved == true)
        // {
        //     visitorVM.ApprovalStatus = ApprovalStatus.Approved.ToString();
        // }
        // else if (visitor.IsApproved == false)
        // {
        //     visitorVM.ApprovalStatus = ApprovalStatus.Rejected.ToString();
        // }
        // else
        // {
        //     visitorVM.ApprovalStatus = ApprovalStatus.Expired.ToString();
        // }

        User? resident = await _userService.CurrentHouseResident(visitorVM.Address);
        if (resident != null)
        {
            visitorVM.ResidentRole = resident.Role.Name;
        }

        if (visitor.VisitorFeedbacks != null)
        {
            visitorVM.Feedback = visitor.VisitorFeedbacks.FirstOrDefault()?.Feedback;
            visitorVM.Rating = visitor.VisitorFeedbacks.FirstOrDefault()?.Rating;
        }

        return visitorVM;
    }

    public List<VisitPurpose> VisitPurposes()
    {
        return _visitPurposeRepository.GetAll().ToList();
    }

    public List<VisitorStatus> VisitorStatuses()
    {
        return _visitorStatusRepository.GetAll().ToList();
    }


    public async Task<VisitorPagination> List(VisitorFilterVM filter)
    {
        //For sorting the column according to order
        Func<IQueryable<Visitor>, IOrderedQueryable<Visitor>>? orderBy = q => q.OrderBy(v => v.Id);

        if (!string.IsNullOrEmpty(filter.Column))
        {
            switch (filter.Column.ToLower())
            {
                case "house":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(u => u.HouseMappingId) : q => q.OrderByDescending(u => u.HouseMappingId);
                    break;
                case "name":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(u => u.Name) : q => q.OrderByDescending(u => u.Name);
                    break;
                case "phone":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(u => u.Phone) : q => q.OrderByDescending(u => u.Phone);
                    break;
                case "purpose":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(u => u.VisitPurpose.Name) : q => q.OrderByDescending(u => u.VisitPurpose.Name);
                    break;
                case "noofvisitors":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(u => u.NoOfVisitors) : q => q.OrderByDescending(u => u.NoOfVisitors);
                    break;
                case "vehicle":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(u => u.VehicleNo) : q => q.OrderByDescending(u => u.VehicleNo);
                    break;
                case "checkin":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(u => u.CheckInTime) : q => q.OrderByDescending(u => u.CheckInTime);
                    break;
                case "checkout":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(u => u.CheckOutTime) : q => q.OrderByDescending(u => u.CheckOutTime);
                    break;
                default:
                    break;
            }
        }

        //Apply filter on the basis of Current logged in user
        filter.Role = _httpService.LoggedInUserRole();

        if (filter.Role == "Owner" || filter.Role == "Tenant")
        {
            int userId = await _httpService.LoggedInUserId();
            filter.Id = await _houseMappingService.GetId(userId);
        }
        else
        {
            filter.Id = -1;
        }

        //Check search string is not null and deleting space 
        filter.Search = string.IsNullOrEmpty(filter.Search) ? "" : filter.Search.Replace(" ", "");
        filter.Search = filter.Search.ToLower();

        Expression<Func<Visitor, bool>> predicate = v => (filter.Id == -1 || v.HouseMappingId == filter.Id) &&      //User filter
                                                    v.DeletedBy == null &&                                          //Not deleted
                                                    (string.IsNullOrEmpty(filter.Search) ||                         //Search query
                                                    v.Name.ToLower().Contains(filter.Search));

        //Filter on the basis of Date Range
        Expression<Func<Visitor, bool>> datePredicate = v => true;      //All

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

        //Filter on the basis of Check Out Status
        Expression<Func<Visitor, bool>> checkOutPredicate = v => true;
        if (!string.IsNullOrEmpty(filter.CheckOutStatus) && filter.CheckOutStatus.ToLower() != "all")
        {
            switch (filter.CheckOutStatus.ToLower())
            {
                case "current":
                    checkOutPredicate = v => v.CheckInTime != null && v.CheckOutTime == null;
                    break;

                case "checked out":
                    checkOutPredicate = v => v.CheckOutTime != null;
                    break;

            }
        }

        //Filter on the basis of Visit Purpose
        Expression<Func<Visitor, bool>> visitPurposePredicate = v => filter.VisitPurpose == 0 || v.VisitPurposeId == filter.VisitPurpose;

        Expression<Func<Visitor, bool>> visitorStatusPredicate = v => filter.VisitorStatus == -1 || v.StatusId == filter.VisitorStatus;

        //Fetching records on the basis of applied filters
        DbResult<Visitor> dbResult = await _visitorRepository.GetRecords(
            predicates: new List<Expression<Func<Visitor, bool>>>
            {
                predicate,
                datePredicate,
                visitorStatusPredicate,
                checkOutPredicate,
                visitPurposePredicate
            },
            orderBy: orderBy,
            includes: new List<Expression<Func<Visitor, object>>>
            {
                v => v.HouseMapping,
                v => v.VisitPurpose,
                v => v.Status
            },
            pageSize: filter.PageSize,
            pageNumber: filter.PageNumber
        );

        List<VisitorVM> visitorList = new();

        //Setting all values in View Model
        foreach (Visitor r in dbResult.Records)
        {
            AddressVM address = await _houseMappingService.GetAddress(r.HouseMappingId);        //Resident Address where visitors wants to go

            string residentRole = "";
            User? resident = await _userService.CurrentHouseResident(address);
            if (resident != null)
            {
                residentRole = resident.Role.Name;
            }

            visitorList.Add(new VisitorVM
            {
                Id = r.Id,
                Name = r.Name,
                Phone = r.Phone,
                VisitPurpose = r.VisitPurpose.Name == "Other" ? r.VisitPurposeReason : r.VisitPurpose.Name,
                NoOfVisitors = r.NoOfVisitors,
                CreatedAt = r.CreatedAt,
                Address = address,
                VehicleNumber = r.VehicleNo,
                ApprovalStatus = r.Status.Name,
                CheckIn = r.CheckInTime,
                CheckOut = r.CheckOutTime,
                ResidentRole = residentRole
            });
        }

        VisitorPagination Visitors = new()
        {
            Visitors = visitorList
        };

        Visitors.Page.SetPagination(dbResult.TotalRecord, filter.PageSize, filter.PageNumber);

        return Visitors;
    }

    public async Task VisitorStatus(int id, bool IsApproved)
    {
        Visitor visitor = await _visitorRepository.GetByIdAsync(id)
                        ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "Visitor"));

        int approvedById = await _httpService.LoggedInUserId();
        string approvedByName = await _httpService.LoggedInUserName();

        if (IsApproved)
        {
            visitor.StatusId = 2;
            visitor.CheckInTime = DateTime.Now;
        }
        else
        {
            visitor.StatusId = 3;
        }

        // visitor.IsApproved = IsApproved;
        visitor.UpdatedAt = DateTime.Now;
        visitor.UpdatedBy = approvedById;

        await _visitorRepository.UpdateAsync(visitor);

        Notification notify = new()         //notify security about approved status
        {
            SenderId = await _httpService.LoggedInUserId(),
            ReceiverId = visitor.CreatedBy,
            TargetId = visitor.Id,
        };

        if (IsApproved)
        {
            notify.Message = NotificationMessages.VisitorApproved.Replace("{0}", approvedByName).Replace("{1}", visitor.Name);
        }
        else
        {
            notify.Message = NotificationMessages.VisitorRejected.Replace("{0}", approvedByName).Replace("{1}", visitor.Name);
        }

        await _notificationService.VisitorNotification(notify);
    }

    public async Task VisitorStatusExpired(int id)
    {
        Visitor visitor = await _visitorRepository.GetByIdAsync(id)
                        ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "Visitor"));

        visitor.StatusId = 4;
        visitor.UpdatedAt = DateTime.Now;
        await _visitorRepository.UpdateAsync(visitor);
    }



    public async Task<ResponseVM> Save(VisitorVM visitorVM)
    {
        ResponseVM response = new();

        int securityId = await _httpService.LoggedInUserId();

        Visitor visitor = await _visitorRepository.GetByIdAsync(visitorVM.Id) ?? new();

        visitor.HouseMappingId = await _houseMappingService.Get(visitorVM.Address);
        visitor.Name = visitorVM.Name;
        visitor.Phone = visitorVM.Phone;
        visitor.NoOfVisitors = visitorVM.NoOfVisitors;
        visitor.VisitPurposeId = visitorVM.VisitPurposeId;
        visitor.VehicleNo = visitorVM.VehicleNumber;
        visitor.UpdatedAt = DateTime.Now;
        visitor.UpdatedBy = securityId;

        //If visit purpose is other then store it in reason
        if (visitorVM.VisitPurposeId == _visitPurposeRepository.GetByStringAsync(vp => vp.Name == "Other").Result!.Id)
        {
            visitor.VisitPurposeReason = visitorVM.VisitReason;
        }
        else
        {
            visitor.VisitPurposeReason = null;
        }

        if (visitorVM.Id == 0)
        {
            //Add Visitor
            visitor.StatusId = 1;
            visitor.CreatedAt = DateTime.Now;
            visitor.CreatedBy = securityId;

            visitor.Id = await _visitorRepository.AddAsyncReturnId(visitor);

            Notification notify = new()
            {
                SenderId = securityId,
                TargetId = visitor.Id,
                Message = NotificationMessages.NewVisitorArrived.Replace("{0}", visitor.Name),
            };

            //Notify House Owner about new visitor
            int ownerRoleId = await _roleService.Get(RoleType.Owner.ToString());
            response = await _userService.GetHouseResident(visitorVM.Address, ownerRoleId);     // to get the resident id
            if (response.Id != 0)
            {
                notify.ReceiverId = response.Id;
                await _notificationService.VisitorNotification(notify);
            }

            //Notify Tenant about new visitor
            int tenantRoleId = await _roleService.Get(RoleType.Tenant.ToString());
            response = await _userService.GetHouseResident(visitorVM.Address, tenantRoleId);
            if (response.Id != 0)
            {
                notify.ReceiverId = response.Id;
                await _notificationService.VisitorNotification(notify);
            }

            response.Success = true;
            response.Message = NotificationMessages.Added.Replace("{0}", "Visitor");
        }
        else
        {
            await _visitorRepository.UpdateAsync(visitor);

            response.Success = true;
            response.Message = NotificationMessages.Updated.Replace("{0}", "Visitor");
        }

        return response;
    }

    public async Task<ResponseVM> CheckOut(int id, int rating, string feedback)
    {
        Visitor visitor = await _visitorRepository.GetByIdAsync(id)
                        ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "Visitor"));

        if (rating != 0 || !string.IsNullOrEmpty(feedback))         //If there is any feedback from visitor
        {
            await _feedbackService.Add(id, rating, feedback);
        }

        int securityId = await _httpService.LoggedInUserId();

        visitor.CheckOutTime = DateTime.Now;
        visitor.UpdatedAt = DateTime.Now;
        visitor.UpdatedBy = securityId;

        await _visitorRepository.UpdateAsync(visitor);

        Notification notify = new()
        {
            SenderId = securityId,
            TargetId = visitor.Id,
            Message = NotificationMessages.VisitorCheckedOut.Replace("{0}", visitor.Name)
        };

        //Resident Address where visitor gone
        AddressVM residentAddress = await _houseMappingService.GetAddress(visitor.HouseMappingId);

        //Notify House Owner about visitor checkout
        int ownerRoleId = await _roleService.Get(RoleType.Owner.ToString());
        ResponseVM response = await _userService.GetHouseResident(residentAddress, ownerRoleId);     // to get the resident id
        if (response.Id != 0)
        {
            notify.ReceiverId = response.Id;
            await _notificationService.VisitorNotification(notify);
        }

        //Notify Tenant about visitor checkout
        int tenantRoleId = await _roleService.Get(RoleType.Tenant.ToString());
        response = await _userService.GetHouseResident(residentAddress, tenantRoleId);
        if (response.Id != 0)
        {
            notify.ReceiverId = response.Id;
            await _notificationService.VisitorNotification(notify);
        }

        response.Success = true;
        response.Message = notify.Message;
        return response;

    }

}
