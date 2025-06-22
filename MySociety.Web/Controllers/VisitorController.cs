using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySociety.Entity.ViewModels;
using MySociety.Service.Common;
using MySociety.Service.Interfaces;

namespace MySociety.Web.Controllers;

[Authorize]
public class VisitorController : Controller
{
    private readonly IVisitorService _visitorService;

    public VisitorController(IVisitorService visitorService)
    {
        _visitorService = visitorService;
    }

    public IActionResult Index()
    {
        VisitorIndexVM index = new()
        {
            VisitPurposes = _visitorService.VisitPurposes()
        };
        ViewData["sidebar-active"] = "Visitors";
        return View(index);
    }

    public async Task<IActionResult> List(StatusFilterVM filter)
    {
        VisitorPagination list = await _visitorService.List(filter);
        return PartialView("_ListPartial", list);
    }

    public async Task<JsonResult> VisitorStatus(int id, bool IsApproved)
    {
        ResponseVM response = await _visitorService.VisitorStatus(id, IsApproved);
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> Get(int id = 0)
    {
        ViewData["sidebar-active"] = "Visitors";

        VisitorVM visitorVM = await _visitorService.Get(id);

        return View(visitorVM);
    }

    [HttpPost]
    public async Task<IActionResult> Save(VisitorVM visitorVM)
    {
        if (visitorVM.VisitPurposeId != 4)
        {
            ModelState.Remove("VisitReason");
        }

        if (!ModelState.IsValid)
        {
            visitorVM = await _visitorService.Get(visitorVM.Id);
            return View("GetVisitor", visitorVM);
        }
        ResponseVM response = await _visitorService.Save(visitorVM);

        TempData["NotificationMessage"] = response.Message;
        TempData["NotificationType"] = NotificationType.Success.ToString();

        if (response.Success)
        {
            return RedirectToAction("Index", "Visitor");
        }
        else
        {
            visitorVM = await _visitorService.Get(visitorVM.Id);
            return View("GetVisitor", visitorVM);
        }
    }

    public async Task<JsonResult> CheckOut(int id, int rating = 0, string feedback = "")
    {
        await _visitorService.CheckOut(id, rating, feedback);
        ResponseVM response = new()
        {
            Success = true,
            Message = NotificationMessages.VisitorCheckedOut
        };
        return Json(response);
    }
}
