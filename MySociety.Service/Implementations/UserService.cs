using Microsoft.AspNetCore.Http;
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

    public async Task<User?> GetByEmail(string email)
    {
        User? user = await _userRepository.GetByStringAsync(u => u.Email == email && u.IsActive == true && u.DeletedBy == null);
        return user;
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

    public async Task<int> LoggedInUser()
    {
        string token = _httpContextAccessor.HttpContext.Request.Cookies["authToken"];
        string userEmail = JwtService.GetClaimValue(token, "email")
                        ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "Logged In User"));

        User user = await GetByEmail(userEmail)
                    ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "Logged In User"));

        return user.Id;
    }



}
