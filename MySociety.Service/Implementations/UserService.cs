using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;
using MySociety.Repository.Interfaces;
using MySociety.Service.Common;
using MySociety.Service.Helper;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class UserService : IUserService
{
    private readonly IGenericRepository<User> _userRepository;

    public UserService(IGenericRepository<User> userRepository)
    {
        _userRepository = userRepository;
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



}
