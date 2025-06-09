using MySociety.Entity.ViewModels;

namespace MySociety.Service.Interfaces;

public interface IAuthService
{
    Task<(LoginResultVM loginResult, ResponseVM response)> Login(LoginVM loginVM);
    Task<ResponseVM> ForgotPassword(string email, string resetToken, string resetLink);
    Task<ResponseVM> ResetPassword(string token, string newPassword);
    Task<ResponseVM> Register(RegisterVM registerVM);
    Task<ResponseVM> VerifyUser(LoginVM loginVM);
}
