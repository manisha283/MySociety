using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;

namespace MySociety.Service.Interfaces;

public interface IUserService
{
    Task<User?> GetByEmail(string email);
    Task<ResponseVM> UpdatePassword(string email, string newPassword);
    Task<ResponseVM> Register(RegisterVM registerVM);
    Task<int> Add(RegisterVM registerVM);
    Task<int> LoggedInUser();
    Task<ResponseVM> CheckUser(string email);
}
