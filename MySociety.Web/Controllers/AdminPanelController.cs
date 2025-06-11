using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySociety.Entity.ViewModels;
using MySociety.Service.Common;
using MySociety.Service.Interfaces;

namespace MySociety.Web.Controllers;

public class AdminPanelController : Controller
{
    private readonly IUserService _userService;
    public AdminPanelController(IUserService userService)
    {
        _userService = userService;
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UserList(FilterVM filter)
    {
        ApprovalPaginationVM list = await _userService.List(filter);
        return PartialView("_ApprovalListPartial", list);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ApproveUser(int userId)
    {
        await _userService.ApproveUser(userId);
        ResponseVM response = new()
        {
            Success = true,
            Message = NotificationMessages.UserApproved
        };
        return Json(response);
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        await _userService.Delete(userId);
        ResponseVM response = new()
        {
            Success = true,
            Message = NotificationMessages.Deleted.Replace("{0}","User")
        };
        return Json(response);
    }
}
