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


    public UserService(IGenericRepository<User> userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    #region Get

    public async Task<User?> GetByEmail(string email)
    {
        User? user = await _userRepository.GetByStringAsync(u => u.Email == email && u.IsActive == true && u.DeletedBy == null);
        return user;
    }

    public async Task<ProfileVM> GetProfile()
    {
        int userId = await LoggedInUser();
        User user = await _userRepository.GetByStringAsync(
            predicate: u => u.Id == userId,
            queries: new List<Func<IQueryable<User>, IQueryable<User>>>
            {
                q => q.Include(u => u.UserHouseMappingUsers)
                        .ThenInclude(uhm => uhm.HouseMapping)
                        .ThenInclude(m => m.Block),
                q => q.Include(u => u.UserHouseMappingUsers)
                        .ThenInclude(uhm => uhm.HouseMapping)
                        .ThenInclude(m => m.Floor),
                q => q.Include(u => u.UserHouseMappingUsers)
                        .ThenInclude(uhm => uhm.HouseMapping)
                        .ThenInclude(m => m.House)
            }
        ) ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "User"));

        ProfileVM profile = new()
        {
            UserId = userId,
            Name = user.Name,
            Phone = user.Phone,
            Email = user.Email,
            Block = user.UserHouseMappingUsers.Select(m => m.HouseMapping.Block.Name).FirstOrDefault()!,
            Floor = user.UserHouseMappingUsers.Select(m => m.HouseMapping.Floor.Name).FirstOrDefault()!,
            House = user.UserHouseMappingUsers.Select(m => m.HouseMapping.House.Name).FirstOrDefault()!,
            ProfileImageUrl = user.ProfileImg
        };

        return profile;
    }
    #endregion

    #region List

    public async Task<ApprovalPaginationVM> List(FilterVM filter)
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
                default:
                    break;
            }
        }

        DbResult<User> users = await _userRepository.GetRecords(
            predicate: u => u.DeletedBy == null && !u.IsApproved && u.IsActive == true &&
                         (string.IsNullOrEmpty(filter.Search.ToLower()) ||
                            u.Role.Name.ToLower() == filter.Search.ToLower() ||
                            u.Name.ToLower().Contains(filter.Search.ToLower())),
            orderBy: orderBy,
            queries: new List<Func<IQueryable<User>, IQueryable<User>>>
            {
                q => q.Include(u => u.UserHouseMappingUsers)
                        .ThenInclude(uhm => uhm.HouseMapping)
                        .ThenInclude(m => m.Block),
                q => q.Include(u => u.UserHouseMappingUsers)
                        .ThenInclude(uhm => uhm.HouseMapping)
                        .ThenInclude(m => m.Floor),
                q => q.Include(u => u.UserHouseMappingUsers)
                        .ThenInclude(uhm => uhm.HouseMapping)
                        .ThenInclude(m => m.House),
                q => q.Include(u => u.Role)
            },
            pageSize: filter.PageSize,
            pageNumber: filter.PageNumber
        );

        ApprovalPaginationVM model = new()
        {
            Users = users.Records.Select(u => new ApproveUserVM()
            {
                UserId = u.Id,
                Email = u.Email,
                Name = u.Name,
                Block = u.UserHouseMappingUsers.Where(u => u.UserId == u.Id).Select(u => u.HouseMapping.Block.Name).FirstOrDefault()!,
                Floor = u.UserHouseMappingUsers.Where(u => u.UserId == u.Id).Select(u => u.HouseMapping.Floor.Name).FirstOrDefault()!,
                House = u.UserHouseMappingUsers.Where(u => u.UserId == u.Id).Select(u => u.HouseMapping.House.Name).FirstOrDefault()!,
                Role = u.Role.Name
            }).ToList()
        };

        model.Page.SetPagination(users.TotalRecord, filter.PageSize, filter.PageNumber);
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
            UpdatedBy = 1
        };

        int userId = await _userRepository.AddAsyncReturnId(user);

        return userId;
    }

    public async Task<ResponseVM> Register(RegisterVM registerVM)
    {
        ResponseVM response = new();
        User? user = await GetByEmail(registerVM.Email);
        if (user != null)
        {
            response.Success = false;

            if (!user.IsApproved)
            {
                response.Message = NotificationMessages.UserNotApproved;
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

    public async Task ApproveUser(int userId)
    {
        User user = await _userRepository.GetByIdAsync(userId)
                    ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "User"));

        user.IsApproved = true;
        user.UpdatedAt = DateTime.Now;
        user.UpdatedBy = await LoggedInUser();

        await _userRepository.UpdateAsync(user);
    }

    public async Task Delete(int userId)
    {
        User user = await _userRepository.GetByIdAsync(userId)
                    ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "User"));

        user.DeletedAt = DateTime.Now;
        user.DeletedBy = await LoggedInUser();

        await _userRepository.UpdateAsync(user);
    }


    #endregion

    #region Common
    public async Task<ResponseVM> CheckUser(string email)
    {
        ResponseVM response = new();
        User? user = await GetByEmail(email);
        if (user != null)
        {
            response.Success = false;

            if (user.IsApproved)
            {
                response.Message = NotificationMessages.UserNotApproved;
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

    public async Task<int> LoggedInUser()
    {
        string token = _httpContextAccessor.HttpContext.Request.Cookies["mySocietyAuthToken"];
        string userEmail = JwtService.GetClaimValue(token, "email")
                        ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "Logged In User"));

        User user = await GetByEmail(userEmail)
                    ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "Logged In User"));

        return user.Id;
    }

    #endregion

    #region Change Password

    public async Task<ResponseVM> ChangePassword(ChangePasswordVM model)
    {
        User user = await _userRepository.GetByIdAsync(await LoggedInUser()) ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "User"));

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
