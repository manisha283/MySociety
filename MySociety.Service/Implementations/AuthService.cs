using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;
using MySociety.Repository.Interfaces;
using MySociety.Service.Common;
using MySociety.Service.Configuration;
using MySociety.Service.Exceptions;
using MySociety.Service.Helper;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<ResetPasswordToken> _resetPasswordRepository;
    private readonly IEmailService _emailService;
    private readonly IJwtService _jwtService;
    private readonly IBlockService _blockService;
    private readonly IUserService _userService;
    private readonly IFloorService _floorService;
    private readonly IHouseService _houseService;
    private readonly IGenericRepository<UserOtp> _userOtpRepository;
    private readonly IHouseMappingService _houseMappingService;

    public AuthService(IEmailService emailService, IJwtService jwtService, IGenericRepository<ResetPasswordToken> resetPasswordRepository, IBlockService blockService, IUserService userService, IFloorService floorService, IHouseService houseService, IGenericRepository<UserOtp> userOtpRepository, IHouseMappingService houseMappingService)
    {
        _emailService = emailService;
        _jwtService = jwtService;
        _resetPasswordRepository = resetPasswordRepository;
        _blockService = blockService;
        _userService = userService;
        _floorService = floorService;
        _houseService = houseService;
        _userOtpRepository = userOtpRepository;
        _houseMappingService = houseMappingService;
    }

    #region Login

    public async Task<ResponseVM> VerifyUser(LoginVM loginVM)
    {
        ResponseVM response = await _userService.ValidExistingUser(loginVM.Email);
        if (!response.Success)
        {
            return response;
        }

        User? user = await _userService.GetByEmail(loginVM.Email);

        if (!PasswordHelper.VerifyPassword(loginVM.Password, user!.Password))
        {
            response.Success = false;
            response.Message = NotificationMessages.Invalid.Replace("{0}", "Credentials");
        }
        else
        {
            response = await SendOtpEmail(loginVM.Email);
        }

        return response;
    }

    public async Task<(LoginResultVM loginResult, ResponseVM response)> Login(LoginVM loginVM)
    {
        LoginResultVM loginResultVM = new();
        ResponseVM response = new();

        User? user = await _userService.GetByEmail(loginVM.Email);
        if (user == null)
        {
            response.Success = false;
            response.Message = NotificationMessages.NotFound.Replace("{0}", "User");
        }
        else
        {
            response = await VerifyOtp(user.Id, loginVM.OtpCode);
            if (response.Success)
            {
                string token = await _jwtService.GenerateToken(loginVM.Email);

                loginResultVM.Token = token;
                loginResultVM.ImageUrl = user.ProfileImg;

                response.Success = true;
            }
        }
        return (loginResultVM, response);
    }

    private async Task<ResponseVM> VerifyOtp(int userId, string otpCode)
    {
        ResponseVM response = new();

        UserOtp? userOtp = await _userOtpRepository.GetByStringAsync(
            predicate: u => !u.IsUsed && u.UserId == userId && u.OtpCode == otpCode,
            orderBy: q => q.OrderBy(u => u.Id),
            firstRecord: false);

        if (userOtp == null)
        {
            response.Success = false;
            response.Message = NotificationMessages.Invalid.Replace("{0}", "OTP");
        }
        else if (userOtp.ExpiryTime.Subtract(DateTime.Now).Ticks <= 0)
        {
            response.Success = false;
            response.Message = NotificationMessages.LinkExpired;
        }
        else
        {
            userOtp.IsUsed = true;
            await _userOtpRepository.UpdateAsync(userOtp);

            response.Success = true;
        }
        return response;
    }

    private async Task<ResponseVM> SendOtpEmail(string email)
    {
        ResponseVM response = new();

        Random generator = new();
        string otpCode = generator.Next(0, 1000000).ToString("D6");

        User user = await _userService.GetByEmail(email) ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", otpCode));

        UserOtp userOtp = new()
        {
            UserId = user.Id,
            OtpCode = otpCode
        };

        await _userOtpRepository.AddAsync(userOtp);

        //Sending otp to user
        string body = EmailTemplateHelper.OtpVerification(otpCode);
        if (await _emailService.SendEmail(email, "OTP Verification", body))
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

    public async Task<ResponseVM> Register(RegisterVM registerVM)
    {
        ResponseVM response = await _userService.ValidNewUser(registerVM);
        if (!response.Success)
        {
            return response;
        }

        registerVM.Id = await _userService.Add(registerVM);

        response.Success = true;
        response.Message = NotificationMessages.UserRegistered;

        // registerVM.Address.UnitId = await _houseMappingService.Get(registerVM.Address);
        // registerVM.Address = await _houseMappingService.GetAddress(registerVM.Address.UnitId);

        registerVM.Address.BlockName = await _blockService.GetName(registerVM.Address.BlockId);
        registerVM.Address.FloorName = await _floorService.GetName(registerVM.Address.FloorId);
        registerVM.Address.HouseName = await _houseService.GetName(registerVM.Address.HouseId);

        //Sending email to user for resetting password
        string body = EmailTemplateHelper.NewUserRegistration(registerVM);

        if (await _emailService.SendEmail(EmailConfig.AdminEmail, "New User Registered", body))
        {
            response.Success = true;
            response.Message = NotificationMessages.EmailSent;
        }
        else
        {
            response.Success = false;
            response.Message = NotificationMessages.EmailSendingFailed;
        }

        body = EmailTemplateHelper.RegisteredSuccessfully(registerVM);

        if (await _emailService.SendEmail(registerVM.Email, "Registration Successfully", body))
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


}
