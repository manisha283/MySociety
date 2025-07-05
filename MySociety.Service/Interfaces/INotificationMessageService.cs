using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;

namespace MySociety.Service.Interfaces;

public interface INotificationService
{
    Task<int> GetUnreadCount();
    List<NotificationCategory> GetNotificationCategories();
    Task<NotificationPagination> List(NotificationFilterVM filter);
    Task<List<NotificationVM>> List();
    Task MarkAsRead(int notificationId);
    Task MarkAllAsRead();
    Task VisitorNotification(Notification notify);
}
