using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;

namespace MySociety.Service.Interfaces;

public interface IUserService
{
    Task<User?> GetByEmail(string email);
    Task<ResponseVM> UpdatePassword(string email, string newPassword);
}
