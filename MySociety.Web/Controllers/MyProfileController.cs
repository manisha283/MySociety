using Microsoft.AspNetCore.Mvc;
using MySociety.Entity.ViewModels;
using MySociety.Service.Common;
using MySociety.Service.Interfaces;

namespace MySociety.Web.Controllers;

public class MyProfileController : Controller
{
    private readonly IUserService _userService;

    public MyProfileController(IUserService userService)
    {
        _userService = userService;
    }

    /*------------------------------------------------------ View My Profile and Update Profile---------------------------------------------------------------------------------
    ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ProfileVM profile = await _userService.GetProfile();
        return View(profile);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ProfileVM profileVM)
    {
        if (!ModelState.IsValid)
        {
            return View(profileVM);
        }

        await _userService.UpdateProfile(profileVM);

        TempData["NotificationMessage"] = NotificationMessages.Updated.Replace("{0}", "Profile");
        TempData["NotificationType"] = NotificationType.Success.ToString();

        return RedirectToAction("Index", "Home");
    }

      
    /*---------------------------------------------------------------Change Password---------------------------------------------------------------------------------
    ----------------------------------------------------------------------------------------------------------------------------------------------------------*/
    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        ResponseVM response = await _userService.ChangePassword(model);

        TempData["NotificationMessage"] = response.Message;
        if (response.Success)
        {
            TempData["NotificationType"] = NotificationType.Success.ToString();
            return RedirectToAction("Logout","Auth");
        }
        else
        {
            TempData["NotificationType"] = NotificationType.Error.ToString();
            return View(model);
        }
    }
}
