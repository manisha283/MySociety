using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
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

public class UserService : IUserService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IHttpService _httpService;
    private readonly IHouseMappingService _houseMappingService;
    private readonly IEmailService _emailService;
    private readonly IRoleService _roleService;

    public UserService(IGenericRepository<User> userRepository, IHttpContextAccessor httpContextAccessor, IHttpService httpService, IHouseMappingService houseMappingService, IEmailService emailService, IRoleService roleService)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _httpService = httpService;
        _houseMappingService = houseMappingService;
        _emailService = emailService;
        _roleService = roleService;

    }

    #region Get

    public async Task<User?> GetByEmail(string email)
    {
        User? user = await _userRepository.GetByStringAsync(u => u.Email == email && u.DeletedBy == null);
        return user;
    }

    public async Task<MemberVM> GetMember(int id)
    {
        User user = await _userRepository.GetByStringAsync(
                    predicate: u => u.Id == id,
                    queries: new List<Func<IQueryable<User>, IQueryable<User>>>
                    {
                        q => q.Include(u => u.Role),
                        q => q.Include(u => u.HouseUnit).ThenInclude(hu => hu.Block),
                        q => q.Include(u => u.HouseUnit).ThenInclude(hu => hu.Floor),
                        q => q.Include(u => u.HouseUnit).ThenInclude(hu => hu.House)
                    }
                    )
                    ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "Member"));

        MemberVM memberVM = new()
        {
            Id = id,
            Email = user.Email,
            Name = user.Name,
            Address = new()
            {
                BlockName = user.HouseUnit.Block.Name,
                FloorName = user.HouseUnit.Floor.Name,
                HouseName = user.HouseUnit.House.Name,
                UnitName = user.HouseUnit.HouseName
            },
            Role = user.Role.Name,
            IsApproved = user.IsApproved,
            RegisteredAt = user.CreatedAt
        };

        return memberVM;
    }

    //Get House resident on the basis of house id and role
    public async Task<ResponseVM> GetHouseResident(AddressVM address, int roleId)
    {
        if (address.UnitId == 0)
        {
            address.UnitId = await _houseMappingService.Get(address);
        }

        User? user = await _userRepository.GetByStringAsync(
                                predicate: u => u.HouseUnitId == address.UnitId &&
                                u.RoleId == roleId &&
                                u.IsActive == true &&
                                u.DeletedBy == null);

        ResponseVM response = new();

        if (user == null)
        {
            response.Id = 0;
            response.Success = false;
        }
        else if (user.IsApproved == null)
        {
            response.Id = user.Id;
            response.Success = true;
            response.Message = NotificationMessages.ApprovalPending;
        }
        else if (user.IsApproved == true)
        {
            response.Id = user.Id;
            response.Success = true;
            response.Message = NotificationMessages.HouseAlreadyRegistered;
        }

        return response;
    }

    public async Task<User?> CurrentHouseResident(AddressVM address)
    {
        if (address.UnitId == 0)
        {
            address.UnitId = await _houseMappingService.Get(address);
        }

        int tenantRoleId = await _roleService.Get(RoleType.Tenant.ToString());

        User? tenant = await _userRepository.GetByStringAsync(
                    predicate: u => u.HouseUnitId == address.UnitId &&
                    u.RoleId == tenantRoleId &&
                    u.IsApproved == true &&
                    u.IsActive == true &&
                    u.DeletedBy == null,
                    queries: new List<Func<IQueryable<User>, IQueryable<User>>>
                    {
                        q => q.Include(u => u.Role)
                    });

        if (tenant != null)
        {
            return tenant;
        }
        else
        {
            int ownerRoleId = await _roleService.Get(RoleType.Owner.ToString());

            User? owner = await _userRepository.GetByStringAsync(
                        predicate: u => u.HouseUnitId == address.UnitId &&
                        u.RoleId == ownerRoleId &&
                        u.IsApproved == true &&
                        u.IsActive == true &&
                        u.DeletedBy == null,
                        queries: new List<Func<IQueryable<User>, IQueryable<User>>>
                        {
                            q => q.Include(u => u.Role)
                        });
                        
            return owner;
        }




    }

    #endregion

    #region List

    public async Task<MembersPagination> List(MemberFilterVM filter)
    {
        filter.Search = string.IsNullOrEmpty(filter.Search) ? "" : filter.Search.Replace(" ", "");

        //For sorting the column according to order
        Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = q => q.OrderBy(u => u.Id);

        if (!string.IsNullOrEmpty(filter.Column))
        {
            switch (filter.Column.ToLower())
            {
                case "name":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(u => u.Name) : q => q.OrderByDescending(u => u.Name);
                    break;
                case "role":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(u => u.Role.Name) : q => q.OrderByDescending(u => u.Role.Name);
                    break;
                case "email":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(u => u.Email) : q => q.OrderByDescending(u => u.Email);
                    break;
                case "block":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(u => u.HouseUnit.Block.BlockNumber).ThenBy(u => u.HouseUnit.Floor.FloorNumber).ThenBy(u => u.HouseUnit.House.HouseNumber)
                                                    : q => q.OrderByDescending(u => u.HouseUnit.Block.BlockNumber).ThenBy(u => u.HouseUnit.Floor.FloorNumber).ThenBy(u => u.HouseUnit.House.HouseNumber);
                    break;
                case "floor":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(u => u.HouseUnit.Floor.FloorNumber).ThenBy(u => u.HouseUnit.Block.BlockNumber).ThenBy(u => u.HouseUnit.House.HouseNumber)
                                                    : q => q.OrderByDescending(u => u.HouseUnit.Floor.FloorNumber).ThenBy(u => u.HouseUnit.Block.BlockNumber).ThenBy(u => u.HouseUnit.House.HouseNumber);
                    break;
                case "house":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(u => u.HouseUnit.House.HouseNumber).ThenBy(u => u.HouseUnit.Block.BlockNumber).ThenBy(u => u.HouseUnit.Floor.FloorNumber)
                                                    : q => q.OrderByDescending(u => u.HouseUnit.House.HouseNumber).ThenBy(u => u.HouseUnit.Block.BlockNumber).ThenBy(u => u.HouseUnit.Floor.FloorNumber);
                    break;
                case "date":
                    orderBy = filter.Sort == "asc" ? q => q.OrderBy(u => u.CreatedAt) : q => q.OrderByDescending(u => u.CreatedAt);
                    break;
                default:
                    break;
            }
        }

        filter.Search = filter.Search.ToLower();

        Expression<Func<User, bool>>? validUserPredicate = u => u.DeletedBy == null;

        Expression<Func<User, bool>>? searchPredicate = u => string.IsNullOrEmpty(filter.Search) ||
                                                            u.Name.ToLower().Contains(filter.Search) ||
                                                            u.Email.ToLower().Contains(filter.Search);

        Expression<Func<User, bool>> statusPredicate = u => true;
        if (!string.IsNullOrEmpty(filter.Status) && filter.Status.ToLower() != "all")
        {
            if (Enum.TryParse<UserStatus>(filter.Status, true, out var status))
            {
                switch (status)
                {
                    case UserStatus.Pending:
                        statusPredicate = u => u.IsApproved == null;
                        break;

                    case UserStatus.Existing:
                        statusPredicate = u => u.IsApproved == true && u.IsActive == true;
                        break;

                    case UserStatus.Rejected:
                        statusPredicate = u => u.IsApproved == false;
                        break;

                    case UserStatus.InActive:
                        statusPredicate = u => u.IsActive == false;
                        break;
                }
            }
        }

        string owner = UserRole.Owner.ToString();
        string tenant = UserRole.Tenant.ToString();

        Expression<Func<User, bool>>? rolePredicate = u => filter.RoleId == -1 ||                                   // all roles if value is -1
                                                        (filter.RoleId == 0 &&                                      // resident (owner or tenant) if value is 0
                                                            (u.Role.Name == owner || u.Role.Name == tenant)) ||
                                                        u.RoleId == filter.RoleId;                                  //specific role

        Expression<Func<User, bool>>? addressPredicate = u => (filter.BlockId == -1 || u.HouseUnit.BlockId == filter.BlockId) &&        //block
                                                            (filter.FloorId == -1 || u.HouseUnit.FloorId == filter.FloorId) &&          //floor
                                                            (filter.HouseId == -1 || u.HouseUnit.HouseId == filter.HouseId);            //house

        //Filter on the basis of Date Range
        Expression<Func<User, bool>> datePredicate = u => true;      //All

        DateOnly now = DateOnly.FromDateTime(DateTime.Now);

        if (Enum.TryParse(filter.DateRange, true, out DateFilterType dateFilter) && filter.DateRange != "all")
        {
            switch (dateFilter)
            {
                case DateFilterType.Today:
                    datePredicate = u =>
                        DateOnly.FromDateTime(u.CreatedAt) == now;
                    break;

                case DateFilterType.Last7Days:
                    DateOnly from7 = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));
                    datePredicate = u =>
                        DateOnly.FromDateTime(u.CreatedAt) >= from7 &&
                        DateOnly.FromDateTime(u.CreatedAt) <= now;
                    break;

                case DateFilterType.Last30Days:
                    DateOnly from30 = DateOnly.FromDateTime(DateTime.Now.AddDays(-30));
                    datePredicate = u =>
                        DateOnly.FromDateTime(u.CreatedAt) >= from30 &&
                        DateOnly.FromDateTime(u.CreatedAt) <= now;
                    break;

                case DateFilterType.CurrentMonth:
                    int currentMonth = DateTime.Now.Month;
                    int currentYear = DateTime.Now.Year;
                    datePredicate = u =>
                        DateOnly.FromDateTime(u.CreatedAt).Month == currentMonth &&
                        DateOnly.FromDateTime(u.CreatedAt).Year == currentYear;
                    break;

                case DateFilterType.CustomDate:
                    if (filter.FromDate.HasValue && filter.ToDate.HasValue)
                    {
                        DateOnly from = filter.FromDate.Value;
                        DateOnly to = filter.ToDate.Value;
                        datePredicate = u =>
                            DateOnly.FromDateTime(u.CreatedAt) >= from &&
                            DateOnly.FromDateTime(u.CreatedAt) <= to;
                    }
                    else if (filter.FromDate.HasValue)
                    {
                        DateOnly from = filter.FromDate.Value;
                        datePredicate = u => DateOnly.FromDateTime(u.CreatedAt) >= from;
                    }
                    else if (filter.ToDate.HasValue)
                    {
                        DateOnly to = filter.ToDate.Value;
                        datePredicate = u => DateOnly.FromDateTime(u.CreatedAt) <= to;
                    }
                    break;
            }
        }

        DbResult<User> dbResult = await _userRepository.GetRecords(
            predicates: new List<Expression<Func<User, bool>>>
            {
                validUserPredicate,
                searchPredicate,
                statusPredicate,
                rolePredicate,
                addressPredicate,
                datePredicate
            },
            orderBy: orderBy,
            queries: new List<Func<IQueryable<User>, IQueryable<User>>>
            {
                q => q.Include(u => u.Role),
                q => q.Include(u => u.HouseUnit).ThenInclude(hu => hu.Block),
                q => q.Include(u => u.HouseUnit).ThenInclude(hu => hu.Floor),
                q => q.Include(u => u.HouseUnit).ThenInclude(hu => hu.House)
            },
            pageSize: filter.PageSize,
            pageNumber: filter.PageNumber
        );

        MembersPagination model = new()
        {
            Members = dbResult.Records.Select(u => new MemberVM()
            {
                Id = u.Id,
                Email = u.Email,
                Name = u.Name,
                Address = new()
                {
                    BlockName = u.HouseUnit?.Block?.Name ?? "-",
                    FloorName = u.HouseUnit?.Floor?.Name ?? "-",
                    HouseName = u.HouseUnit?.House?.Name ?? "-",
                },
                Role = u.Role.Name,
                IsApproved = u.IsApproved,
                RegisteredAt = u.CreatedAt

            }).ToList()
        };

        model.Page.SetPagination(dbResult.TotalRecord, filter.PageSize, filter.PageNumber);
        return model;
    }

    #endregion

    #region Update

    public async Task<int> Add(RegisterVM registerVM)
    {
        User user = new()
        {
            Name = registerVM.Name,
            Email = registerVM.Email,
            Password = PasswordHelper.HashPassword(registerVM.Password),
            RoleId = registerVM.RoleId,
            CreatedBy = 1,
            UpdatedBy = 1,
            HouseUnitId = await _houseMappingService.Get(registerVM.Address)
        };

        int userId = await _userRepository.AddAsyncReturnId(user);

        return userId;
    }

    public async Task<ResponseVM> Register(RegisterVM registerVM)
    {
        ResponseVM response = await ValidNewUser(registerVM);
        if (response.Success)
        {
            await Add(registerVM);

            response.Success = true;
            response.Message = NotificationMessages.UserRegistered;
        }

        return response;
    }

    public async Task<ResponseVM> UpdatePassword(string email, string newPassword)
    {
        User? user = await GetByEmail(email);
        ResponseVM response = new();
        if (user == null)
        {
            response.Success = false;
            response.Message = NotificationMessages.NotFound.Replace("{0}", "User");
        }
        else
        {
            user.Password = PasswordHelper.HashPassword(newPassword);
            await _userRepository.UpdateAsync(user);
            response.Success = true;
            response.Message = NotificationMessages.Updated.Replace("{0}", "Password");
        }
        return response;
    }

    public async Task UpdateProfile(ProfileVM profile)
    {
        User user = await _userRepository.GetByIdAsync(profile.UserId)
                    ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "User"));

        user.Name = profile.Name;
        user.Phone = profile.Phone;

        // Handle Image Upload
        if (profile.Image != null)
        {
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profile");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string fileName = $"{Guid.NewGuid()}_{profile.Image.FileName}";
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profile.Image.CopyToAsync(stream);
            }

            user.ProfileImg = $"/images/profile/{fileName}";
        }

        await _userRepository.UpdateAsync(user);

        CookieOptions options = new()
        {
            Expires = DateTime.Now.AddDays(1),
            HttpOnly = true,
            IsEssential = true
        };

        if (profile.Image != null)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("mySocietyProfileImg");
            _httpContextAccessor.HttpContext.Response.Cookies.Append("mySocietyProfileImg", profile.ProfileImageUrl, options);
        }

        if (profile.Name != user.Name)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("mySocietyUserName");
            _httpContextAccessor.HttpContext.Response.Cookies.Append("mySocietyUserName", profile.Name, options);
        }

    }

    public async Task<ResponseVM> ChangeUserStatus(int userId, bool isApprove)
    {
        User user = await _userRepository.GetByIdAsync(userId)
                    ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "User"));

        user.IsApproved = isApprove;
        user.UpdatedAt = DateTime.Now;
        user.UpdatedBy = await _httpService.LoggedInUserId();

        await _userRepository.UpdateAsync(user);

        ResponseVM response = new()
        {
            Success = true,
            Message = isApprove ? NotificationMessages.Approved.Replace("{0}", "User") : NotificationMessages.Rejected.Replace("{0}", "User")
        };

        string body;
        if (isApprove)  //Sending email to user after admin change approval status
        {
            body = EmailTemplateHelper.NewUserApproved(user.Name);
        }
        else
        {
            body = EmailTemplateHelper.NewUserRejected(user.Name);
        }

        bool emailSent = await _emailService.SendEmail(user.Email, "Admin Approval Status", body);
        if (!emailSent)
        {
            response.Success = false;
            response.Message = NotificationMessages.EmailSendingFailed;
        }

        return response;
    }

    #endregion

    #region Common

    public async Task<ResponseVM> ValidNewUser(RegisterVM registerVM)
    {
        ResponseVM response = await GetHouseResident(registerVM.Address, registerVM.RoleId);

        if (response.Id != 0)
        {
            response.Success = false;
            return response;
        }

        User? user = await GetByEmail(registerVM.Email);

        if (user != null)
        {
            response.Success = false;

            if (user.IsApproved == null)
            {
                response.Message = NotificationMessages.UserPending;
            }
            else if (user.IsApproved == false)
            {
                response.Message = NotificationMessages.UserRejected;
            }
            else if (user.IsActive == false)
            {
                response.Message = NotificationMessages.UserNotActive;
            }
            else
            {
                response.Message = NotificationMessages.AlreadyExisted.Replace("{0}", "User");
            }
        }
        else
        {
            response.Success = true;
        }
        return response;
    }

    public async Task<ResponseVM> ValidExistingUser(string email)
    {
        ResponseVM response = new();
        User? user = await GetByEmail(email);
        if (user == null)
        {
            response.Success = false;
            response.Message = NotificationMessages.NotFound.Replace("{0}", "User");
        }
        else
        {
            if (user.IsApproved == null)
            {
                response.Success = false;
                response.Message = NotificationMessages.UserPending;
            }
            else if (user.IsApproved == false)
            {
                response.Success = false;
                response.Message = NotificationMessages.UserRejected;
            }
            else if (user.IsActive == false)
            {
                response.Success = false;
                response.Message = NotificationMessages.UserNotActive;
            }
            else
            {
                response.Success = true;
            }
        }
        return response;
    }
    #endregion

    #region Change Password

    public async Task<ResponseVM> ChangePassword(ChangePasswordVM model)
    {
        User user = await _userRepository.GetByIdAsync(await _httpService.LoggedInUserId()) ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "User"));

        ResponseVM response = new();

        //Verify Password
        if (!PasswordHelper.VerifyPassword(model.OldPassword, user.Password))
        {
            response.Success = false;
            response.Message = NotificationMessages.Invalid.Replace("{0}", "Old Password");
            return response;
        }

        //Hash and Update Password
        user.Password = PasswordHelper.HashPassword(model.NewPassword);
        await _userRepository.UpdateAsync(user);

        response.Success = true;
        response.Message = NotificationMessages.Updated.Replace("{0}", "Password");
        return response;
    }

    #endregion
}
