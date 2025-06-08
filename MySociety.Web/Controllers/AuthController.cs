using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySociety.Entity.ViewModels;
using MySociety.Service.Common;
using MySociety.Service.Exceptions;
using MySociety.Service.Interfaces;

namespace MySociety.Web.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly IBlockService _blockService;
    private readonly IRoleService _roleService;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService, IBlockService blockService, IRoleService roleService, IUserService userService)
    {
        _authService = authService;
        _blockService = blockService;
        _roleService = roleService;
        _userService = userService;

    }

    #region  Login
    [HttpGet]
    public IActionResult Login()
    {
        if (Request.Cookies["mySocietyEmail"] != null)
        {
            return RedirectToAction("Index", "Home");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (!ModelState.IsValid)
        {
            return View(loginVM);
        }

        (LoginResultVM loginResult, ResponseVM response) = await _authService.Login(loginVM);

        if (!response.Success)
        {
            TempData["NotificationMessage"] = response.Message;
            TempData["NotificationType"] = NotificationType.Error.ToString();
            return View(loginVM);
        }

        if (loginResult.Token != null || loginResult.Token != "")
        {
            CookieOptions options = new()
            {
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true,
                IsEssential = true,
                Secure = true
            };

            Response.Cookies.Append("mySocietyAuthToken", loginResult.Token!, options);
            Response.Cookies.Append("mySocietyProfileImg", loginResult.ImageUrl ?? Images.ProfileImg, options);

            if (loginVM.RememberMe)
            {
                Response.Cookies.Append("mySocietyEmail", loginVM.Email, options);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        TempData["NotificationMessage"] = response.Message;
        TempData["NotificationType"] = NotificationType.Error.ToString();
        return View(loginVM);
    }
    #endregion Login

    #region  Register
    [HttpGet]
    public async Task<IActionResult> Register()
    {
        RegisterVM registerVM = new()
        {
            Roles = _roleService.List(),
            Blocks = await _blockService.List()
        };
        return View(registerVM);
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if (!ModelState.IsValid)
        {
            TempData["NotificationMessage"] = NotificationMessages.ModelStateInvalid;
            TempData["NotificationType"] = NotificationType.Error.ToString();
            return RedirectToAction("Register");
        }

        ResponseVM response = await _authService.Register(registerVM);

        TempData["NotificationMessage"] = response.Message;
        if (response.Success)
        {
            TempData["NotificationType"] = NotificationType.Success.ToString();
            return RedirectToAction("Login", "Auth");
        }
        else
        {
            TempData["NotificationType"] = NotificationType.Error.ToString();
            return RedirectToAction("Register", "Auth");
        }
    }
    #endregion Register

    #region Forgot Password
    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Generate a secure token for reset password (GUID-based)
        string resetToken = Guid.NewGuid().ToString();
        string resetLink = Url.Action("ResetPassword", "Auth", new { token = resetToken }, Request.Scheme)
                        ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "Token")); ;

        ResponseVM response = await _authService.ForgotPassword(model.Email, resetToken, resetLink);
        if (!response.Success)
        {
            TempData["NotificationMessage"] = response.Message;
            TempData["NotificationType"] = NotificationType.Error.ToString();
            return View(model);
        }

        TempData["NotificationMessage"] = response.Message;
        TempData["NotificationType"] = NotificationType.Success.ToString();
        return RedirectToAction("Login", "Auth");
    }
    #endregion Forgot Password

    #region Reset Password
    [HttpGet]
    public IActionResult ResetPassword(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login", "Auth");
        }

        ResetPasswordVM model = new()
        {
            Token = token
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        ResponseVM response = await _authService.ResetPassword(model.Token, model.NewPassword);
        if (!response.Success)
        {
            TempData["NotificationMessage"] = response.Message;
            TempData["NotificationType"] = NotificationType.Error.ToString();
            return View(model);
        }

        TempData["NotificationMessage"] = response.Message;
        TempData["NotificationType"] = NotificationType.Success.ToString();
        return RedirectToAction("Login", "Auth");
    }
    #endregion Reset Password

    #region Logout
    public IActionResult Logout()
    {
        if (Request.Cookies["mySocietyEmail"] != null)
        {
            Response.Cookies.Delete("mySocietyAuthToken");
            Response.Cookies.Delete("mySocietyEmail");
            Response.Cookies.Delete("mySocietyProfileImg");
        }
        return RedirectToAction("Login", "Auth");
    }

    #endregion


}
