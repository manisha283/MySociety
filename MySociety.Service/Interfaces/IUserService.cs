using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;

namespace MySociety.Service.Interfaces;

public interface IUserService
{
    Task<User?> GetByEmail(string email);
    Task<ResponseVM> GetHouseResident(AddressVM address, int roleId);
    Task<User?> CurrentHouseResident(AddressVM address);
    Task<MemberVM> GetMember(int id);
    Task<ResponseVM> Register(RegisterVM registerVM);
    Task<int> Add(RegisterVM registerVM);
    Task<ResponseVM> UpdatePassword(string email, string newPassword);
    Task UpdateProfile(ProfileVM profile);
    Task<ResponseVM> ChangePassword(ChangePasswordVM model);
    Task<MembersPagination> List(MemberFilterVM filter);
    Task<List<MemberVM>> List();
    Task<ResponseVM> ChangeUserStatus(int userId, bool isApprove);
    Task<ResponseVM> ValidNewUser(RegisterVM registerVM);
    Task<ResponseVM> ValidExistingUser(string email);
    
}
