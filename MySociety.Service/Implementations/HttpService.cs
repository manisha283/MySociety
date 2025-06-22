using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MySociety.Entity.Models;
using MySociety.Repository.Interfaces;
using MySociety.Service.Common;
using MySociety.Service.Exceptions;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class HttpService : IHttpService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGenericRepository<User> _userRepository;

    public HttpService(IHttpContextAccessor httpContextAccessor, IGenericRepository<User> userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    public string GetToken()
    {
        return _httpContextAccessor.HttpContext.Request.Cookies["mySocietyAuthToken"];
    }

    public async Task<User> GetUser()
    {
        string token = GetToken();
        string email = JwtService.GetClaimValue(token, "email")
                        ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "Logged In User"));

        User user = await _userRepository.GetByStringAsync(
                    predicate: u => u.Email == email
                    && u.IsActive == true
                    && u.DeletedBy == null
                    && u.IsApproved == true)
                    ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "Logged In User"));

        return user;
    }

    public async Task<int> LoggedInUserId()
    {
        User user = await GetUser();
        return user.Id;
    }

    public string LoggedInUserRole()
    {
        string token = GetToken();

        string role = JwtService.GetClaimValue(token, "role")
                    ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "Role of User"));
                    
        return role;
    }
}
