using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySociety.Entity.ViewModels;
using MySociety.Service.Common;
using MySociety.Service.Interfaces;

namespace MySociety.Web.Controllers;

[Authorize(Roles = "Admin")]
public class MembersController : Controller
{
    private readonly IUserService _userService;
    private readonly IHouseMappingService _houseMappingService;
    private readonly IRoleService _roleService;

    public MembersController(IUserService userService, IHouseMappingService houseMappingService, IRoleService roleService)
    {
        _userService = userService;
        _houseMappingService = houseMappingService;
        _roleService = roleService;
    }

    public async Task<IActionResult> Index()
    {
        MemberIndexVM index = new()
        {
            Roles = _roleService.List(),
            Address = await _houseMappingService.List()
        };

        ViewData["sidebar-active"] = "Members";
        return View(index);
    }

    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        ViewData["sidebar-active"] = "Members";
        MemberVM member = await _userService.GetMember(id);
        return PartialView(member);
    }

    [HttpPost]
    public async Task<IActionResult> List(MemberFilterVM filter)
    {
        MembersPagination list = await _userService.List(filter);
        return PartialView("_ListPartial", list);
    }

    public async Task<IActionResult> ChangeUserStatus(int userId, bool isApprove)
    {
        await _userService.ChangeUserStatus(userId, isApprove);
        ResponseVM response = new()
        {
            Success = true,
            Message = NotificationMessages.Approved.Replace("{0}", "User")
        };
        return Json(response);
    }
}
