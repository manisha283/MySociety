using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;
using MySociety.Service.Interfaces;
using MySociety.Web.Hubs;

namespace MySociety.Web.Controllers;

public class NotificationsController : Controller
{
    private readonly INotificationService _notificationService;
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationsController(INotificationService notificationService, IHubContext<NotificationHub> hubContext)
    {
        _notificationService = notificationService;
        _hubContext = hubContext;
    }

    public async Task SendToUserAsync(string userId, string message)
    {
        await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", message);
    }

    public IActionResult Index()
    {
        List<NotificationCategory> list = _notificationService.GetNotificationCategories();
        ViewData["sidebar-active"] = "Notifications";
        return View(list);
    }

    public async Task<IActionResult> List(NotificationFilterVM filter)
    {
        NotificationPagination list = await _notificationService.List(filter);
        return PartialView("_ListPartial", list);
    }

    public async Task<IActionResult> GetNotifications()
    {
        List<NotificationVM> list = await _notificationService.List();
        return PartialView("_NotificationDropdownPartial", list);
    }

    [HttpGet]
    public async Task<IActionResult> GetUnreadCount()
    {
        int count = await _notificationService.GetUnreadCount();
        return Ok(count);
    }

    public async Task<IActionResult> MarkAsRead(int id)
    {
        await _notificationService.MarkAsRead(id);
        return Ok();
    }

    public async Task<IActionResult> MarkAllAsRead()
    {
        await _notificationService.MarkAllAsRead();
        return Ok();
    }

}
