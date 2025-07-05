using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySociety.Entity.ViewModels;
using MySociety.Service.Common;
using MySociety.Service.Interfaces;

namespace MySociety.Web.Controllers;

public class NoticesController : Controller
{
    private readonly INoticeService _noticeService;
    private readonly IRoleService _roleService;
    private readonly IBlockService _blockService;
    private readonly IFloorService _floorService;

    public NoticesController(INoticeService noticeService, IRoleService roleService, IBlockService blockService, IFloorService floorService)
    {
        _noticeService = noticeService;
        _roleService = roleService;
        _blockService = blockService;
        _floorService = floorService;

    }

    public async Task<IActionResult> Index()
    {
        NoticeIndexVM index = new()
        {
            Categories = await _noticeService.GetNoticeCategories(),
            Audiences = _noticeService.GetNoticeAudiences()
        };
        ViewData["sidebar-active"] = "Notices";
        return View(index);
    }

    [HttpPost]
    public async Task<IActionResult> List(NoticeFilterVM filter)
    {
        NoticePagination list = await _noticeService.List(filter);
        return PartialView("_ListPartial", list);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Save(int id = 0)
    {
        NoticeVM noticeVM = await _noticeService.Get(id);
        await PopulateNoticeVM(noticeVM);
        ViewData["sidebar-active"] = "Notices";
        return View(noticeVM);
    }

    [Authorize(Roles = "Admin")]
    private async Task PopulateNoticeVM(NoticeVM noticeVM)
    {
        noticeVM.Categories = await _noticeService.GetNoticeCategories();
        noticeVM.AudiencesVM.Roles = _roleService.List();
        noticeVM.AudiencesVM.Blocks = await _blockService.List();
        noticeVM.AudiencesVM.Floors = await _floorService.List();
    }


    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Save(NoticeVM noticeVM)
    {
        if (!ModelState.IsValid)
        {
            TempData["NotificationMessage"] = NotificationMessages.ModelStateInvalid;
            TempData["NotificationType"] = NotificationType.Error.ToString();
            await PopulateNoticeVM(noticeVM);
            return View(noticeVM);
        }

        ResponseVM response = await _noticeService.Save(noticeVM);

        TempData["NotificationMessage"] = response.Message;
        if (response.Success)
        {
            TempData["NotificationType"] = NotificationType.Success.ToString();
            return RedirectToAction("Index");
        }
        else
        {
            TempData["NotificationType"] = NotificationType.Error.ToString();
            await PopulateNoticeVM(noticeVM);
            return View(noticeVM);
        }
    }

    public async Task<IActionResult> Delete(int vehicleId)
    {
        await _noticeService.Delete(vehicleId);
        return Json(new ResponseVM
        {
            Success = true,
            Message = NotificationMessages.Deleted.Replace("{0}", "Vehicle")
        });
    }

}
