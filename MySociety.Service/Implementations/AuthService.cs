using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;
using MySociety.Repository.Interfaces;
using MySociety.Service.Common;
using MySociety.Service.Exceptions;
using MySociety.Service.Helper;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<ResetPasswordToken> _resetPasswordRepository;
    private readonly IEmailService _emailService;
    private readonly IJwtService _jwtService;
    private readonly IBlockService _blockService;
    private readonly IUserService _userService;

    public AuthService(IGenericRepository<User> userRepository, IEmailService emailService, IJwtService jwtService, IGenericRepository<ResetPasswordToken> resetPasswordRepository, IBlockService blockService, IUserService userService)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _jwtService = jwtService;
        _resetPasswordRepository = resetPasswordRepository;
        _blockService = blockService;
        _userService = userService;

    }

    #region Login
    public async Task<(LoginResultVM loginResult, ResponseVM response)> Login(LoginVM loginVM)
    {
        // User user = await _userRepository.GetByStringAsync(u => u.Email == loginVM.Email && u.DeletedBy == null  && u.IsActive == true) 
        //             ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "User"));      
        LoginResultVM loginResultVM = new();
        ResponseVM response = new();

        User? user = await _userService.GetByEmail(loginVM.Email);
        if (user == null)
        {
            response.Success = false;
            response.Message = NotificationMessages.NotFound.Replace("{0}", "User");
            return (loginResultVM, response);
        }
        else if (!PasswordHelper.VerifyPassword(loginVM.Password, user.Password))
        {
            response.Success = false;
            response.Message = NotificationMessages.Invalid.Replace("{0}", "Credentials");
            return (loginResultVM, response);
        }
        else
        {
            string token = await _jwtService.GenerateToken(loginVM.Email);

            loginResultVM.Token = token;
            loginResultVM.UserName = user.Username;
            loginResultVM.ImageUrl = user.ProfileImg;

            response.Success = true;
            return (loginResultVM, response);
        }
    }
    #endregion

    #region ForgotPassword
    public async Task<ResponseVM> ForgotPassword(string email, string resetToken, string resetLink)
    {
        User? user = await _userService.GetByEmail(email);
        ResponseVM response = new();
        if (user == null)
        {
            response.Success = false;
            response.Message = NotificationMessages.NotFound.Replace("{0}", "User");
            return response;
        }

        //Stores the reset password token in table  
        ResetPasswordToken token = new()
        {
            Email = email,
            Token = resetToken
        };

        await _resetPasswordRepository.AddAsync(token);

        //Sending email to user for resetting password
        string body = EmailTemplateHelper.ResetPassword(resetLink);
        if (await _emailService.SendEmail(email, "Reset Password", body))
        {
            response.Success = true;
            response.Message = NotificationMessages.EmailSent;
        }
        else
        {
            response.Success = false;
            response.Message = NotificationMessages.EmailSendingFailed;
        }

        return response;
    }

    #endregion

    #region ResetPassword
    public async Task<ResponseVM> ResetPassword(string token, string newPassword)
    {
        ResetPasswordToken? resetToken = await _resetPasswordRepository.GetByStringAsync(t => t.Token == token);
        ResponseVM response = new();

        if (resetToken == null)
        {
            response.Success = false;
            response.Message = NotificationMessages.NotFound.Replace("{0}", "Token");
        }
        else if (resetToken.Expirytime.Subtract(DateTime.Now).Ticks <= 0)
        {
            response.Success = false;
            response.Message = NotificationMessages.LinkExpired;
        }
        else if (resetToken.IsUsed)     //Check if token is already used
        {
            response.Success = false;
            response.Message = NotificationMessages.AlreadyUsed;
        }
        else
        {
            User? user = await _userService.GetByEmail(resetToken.Email);
            if (user == null)
            {
                response.Success = false;
                response.Message = NotificationMessages.NotFound.Replace("{0}", "User");
            }
            else
            {
                // Updates Password
                response = await _userService.UpdatePassword(resetToken.Email, newPassword);

                // Change token status to used
                resetToken.IsUsed = true;
                await _resetPasswordRepository.UpdateAsync(resetToken);
            }
        }
        return response;
    }

    #endregion

    #region Register


    #endregion Register
}
