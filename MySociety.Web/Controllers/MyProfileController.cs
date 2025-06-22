using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySociety.Entity.ViewModels;
using MySociety.Service.Common;
using MySociety.Service.Interfaces;

namespace MySociety.Web.Controllers;

[Authorize]
public class MyProfileController : Controller
{
    private readonly IUserService _userService;
    private readonly IVehicleService _vehicleService;
    private readonly IProfileService _profileService;

    public MyProfileController(IUserService userService, IVehicleService vehicleService, IProfileService profileService)
    {
        _userService = userService;
        _vehicleService = vehicleService;
        _profileService = profileService;
    }

    /*------------------------------------------------------ View My Profile and Update Profile---------------------------------------------------------------------------------
    ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ProfileVM profile = await _profileService.GetProfile();
        return View(profile);
    }

    [HttpPost]
    public async Task<IActionResult> VehicleList(FilterVM filter)
    {
        VehiclePagination list = await _vehicleService.List(filter);
        return PartialView("_VehicleListPartial", list);
    }

    [HttpPost]
    public async Task<IActionResult> Update(ProfileVM profileVM)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", profileVM);
        }

        await _userService.UpdateProfile(profileVM);

        TempData["NotificationMessage"] = NotificationMessages.Updated.Replace("{0}", "Profile");
        TempData["NotificationType"] = NotificationType.Success.ToString();

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public async Task<IActionResult> VehicleModal(int vehicleId)
    {
        VehicleVM vehicleVM = await _vehicleService.Get(vehicleId);
        return PartialView("_VehiclePartial", vehicleVM);
    }

    [HttpPost]
    public async Task<IActionResult> SaveVehicle(VehicleVM vehicleVM)
    {
        if (!ModelState.IsValid)
        {
            VehicleVM updatedVehicleVM = await _vehicleService.Get(vehicleVM.Id);
            return PartialView("_VehiclePartial", updatedVehicleVM);
        }

        ResponseVM response = await _vehicleService.Save(vehicleVM);
        return Json(response);
    }

    public async Task<IActionResult> DeleteVehicle(int vehicleId)
    {
        await _vehicleService.Delete(vehicleId);
        return Json(new ResponseVM
        {
            Success = true,
            Message = NotificationMessages.Deleted.Replace("{0}", "Vehicle")
        });
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
            return RedirectToAction("Logout", "Auth");
        }
        else
        {
            TempData["NotificationType"] = NotificationType.Error.ToString();
            return View(model);
        }
    }
}
