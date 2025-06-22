using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;

namespace MySociety.Service.Interfaces;

public interface IUserService
{
    Task<User?> GetByEmail(string email);
    Task<ResponseVM> Register(RegisterVM registerVM);
    Task<int> Add(RegisterVM registerVM);
    Task<ResponseVM> UpdatePassword(string email, string newPassword);
    Task UpdateProfile(ProfileVM profile);
    Task<ResponseVM> ChangePassword(ChangePasswordVM model);
    Task<ResponseVM> CheckUser(string email);
    Task<ApprovalPaginationVM> List(FilterVM filter);
    Task<ResponseVM> ChangeUserStatus(int userId, bool isApprove);
}
