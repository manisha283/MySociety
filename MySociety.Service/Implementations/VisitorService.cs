using System.Linq.Expressions;
using System.Threading.Tasks;
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

    public VisitorService(IGenericRepository<Visitor> visitorRepository, IGenericRepository<VisitPurpose> visitPurposeRepository, IBlockService blockService, IHouseMappingService houseMappingService, IHttpService httpService, IVisitorFeedbackService feedbackService)
    {
        _visitorRepository = visitorRepository;
        _visitPurposeRepository = visitPurposeRepository;
        _blockService = blockService;
        _houseMappingService = houseMappingService;
        _httpService = httpService;
        _feedbackService = feedbackService;

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

        Visitor? visitor = await _visitorRepository.GetByIdAsync(id);

        if (visitor == null)
        {
            return visitorVM;
        }

        visitorVM.Name = visitor.Name;
        visitorVM.Phone = visitor.Phone;
        visitorVM.VisitPurposeId = visitor.VisitPurposeId;
        visitorVM.VisitReason = visitor.VisitPurposeReason;
        visitorVM.NoOfVisitors = visitor.NoOfVisitors;
        visitorVM.VehicleNumber = visitor.VehicleNo;

        return visitorVM;
    }

    public List<VisitPurpose> VisitPurposes()
    {
        return _visitPurposeRepository.GetAll().ToList();
    }

    public async Task<VisitorPagination> List(StatusFilterVM filter)
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
            filter.Id = await _houseMappingService.GetId(await _httpService.LoggedInUserId());
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

        //Filter on the basis of Approval status
        Expression<Func<Visitor, bool>> approvePredicate = v => true;
        if (!string.IsNullOrEmpty(filter.Status) && filter.Status.ToLower() != "all")
        {
            if (Enum.TryParse<ApprovalStatus>(filter.Status, true, out var status))
            {
                switch (status)
                {
                    case ApprovalStatus.Pending:
                        approvePredicate = v => v.IsApproved == null && (DateTime.Now - v.CreatedAt).TotalMinutes <= 30;
                        break;

                    case ApprovalStatus.Approved:
                        approvePredicate = v => v.IsApproved == true;
                        break;

                    case ApprovalStatus.Rejected:
                        approvePredicate = v => v.IsApproved == false;
                        break;

                    case ApprovalStatus.Expired:
                        approvePredicate = v => v.IsApproved == null && (DateTime.Now - v.CreatedAt).TotalMinutes >= 30;
                        break;
                }
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


        //Fetching records on the basis of applied filters
        DbResult<Visitor> dbResult = await _visitorRepository.GetRecords(
            predicates: new List<Expression<Func<Visitor, bool>>>
            {
                predicate,
                datePredicate,
                approvePredicate,
                checkOutPredicate,
                visitPurposePredicate
            },
            orderBy: orderBy,
            includes: new List<Expression<Func<Visitor, object>>>
            {
                v => v.HouseMapping,
                v => v.VisitPurpose
            },
            pageSize: filter.PageSize,
            pageNumber: filter.PageNumber
        );

        List<VisitorInfoVM> visitorList = new();

        //Setting all values in View Model
        foreach (Visitor r in dbResult.Records)
        {
            AddressVM address = await _houseMappingService.GetAddress(r.HouseMappingId);        //Resident Address where visitors wants to go

            string approvalStatus;
            if (r.IsApproved == null && (DateTime.Now - r.CreatedAt).TotalMinutes <= 30)
            {
                approvalStatus = ApprovalStatus.Pending.ToString();
            }
            else if (r.IsApproved == true)
            {
                approvalStatus = ApprovalStatus.Approved.ToString();
            }
            else if (r.IsApproved == false)
            {
                approvalStatus = ApprovalStatus.Rejected.ToString();
            }
            else
            {
                approvalStatus = ApprovalStatus.Expired.ToString();
            }

            visitorList.Add(new VisitorInfoVM
            {
                Id = r.Id,
                Name = r.Name,
                Phone = r.Phone,
                VisitPurpose = r.VisitPurpose.Name == "Other" ? r.VisitPurposeReason : r.VisitPurpose.Name,
                NoOfVisitors = r.NoOfVisitors,
                WaitingSince = r.CreatedAt,
                Address = address,
                Vehicle = r.VehicleNo,
                IsApprove = r.IsApproved,
                ApprovalStatus = approvalStatus,
                CheckIn = r.CheckInTime,
                CheckOut = r.CheckOutTime
            });
        }

        VisitorPagination Visitors = new()
        {
            Visitors = visitorList
        };

        Visitors.Page.SetPagination(dbResult.TotalRecord, filter.PageSize, filter.PageNumber);

        return Visitors;
    }

    public async Task<ResponseVM> VisitorStatus(int id, bool IsApproved)
    {
        Visitor visitor = await _visitorRepository.GetByIdAsync(id)
                        ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "Visitor"));

        if (IsApproved)
        {
            visitor.CheckInTime = DateTime.Now;
        }
        visitor.IsApproved = IsApproved;
        visitor.UpdatedAt = DateTime.Now;
        visitor.UpdatedBy = await _httpService.LoggedInUserId();

        await _visitorRepository.UpdateAsync(visitor);

        ResponseVM response = new()
        {
            Success = true
        };

        if (IsApproved)
        {
            response.Message = NotificationMessages.Approved.Replace("{0}", "Visitor");
        }
        else
        {
            response.Message = NotificationMessages.Rejected.Replace("{0}", "Visitor");
        }

        return response;
    }

    public async Task<ResponseVM> Save(VisitorVM visitorVM)
    {
        ResponseVM response = new();

        Visitor visitor = await _visitorRepository.GetByIdAsync(visitorVM.Id) ?? new();

        visitor.HouseMappingId = await _houseMappingService.Get(visitorVM.Address);
        visitor.Name = visitorVM.Name;
        visitor.Phone = visitorVM.Phone;
        visitor.NoOfVisitors = visitorVM.NoOfVisitors;
        visitor.VisitPurposeId = visitorVM.VisitPurposeId;
        visitor.VehicleNo = visitorVM.VehicleNumber;
        visitor.UpdatedAt = DateTime.Now;
        visitor.UpdatedBy = await _httpService.LoggedInUserId();

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
            visitor.CreatedAt = DateTime.Now;
            visitor.CreatedBy = await _httpService.LoggedInUserId();

            await _visitorRepository.AddAsync(visitor);

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

    public async Task CheckOut(int id, int rating, string feedback)
    {
        Visitor visitor = await _visitorRepository.GetByIdAsync(id)
                        ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "Visitor"));

        if (rating != 0 || !string.IsNullOrEmpty(feedback))         //If there is any feedback from visitor
        {
            await _feedbackService.Add(id, rating, feedback);
        }

        visitor.CheckOutTime = DateTime.Now;
        visitor.UpdatedAt = DateTime.Now;
        visitor.UpdatedBy = await _httpService.LoggedInUserId();

        await _visitorRepository.UpdateAsync(visitor);
    }

}
