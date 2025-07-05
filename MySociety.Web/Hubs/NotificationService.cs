using Microsoft.AspNetCore.SignalR;
using MySociety.Service.Interfaces;
namespace MySociety.Web.Hubs;

public class NotificationHubService : INotificationHubService
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationHubService(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyUser(int userId, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", message);
    }
}
