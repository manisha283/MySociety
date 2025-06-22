using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MySociety.Web.Hubs;

[Authorize]
public class NotificationHub : Hub
{
    public async Task SendNotificationToUser(string targetUserId, string message)
    {
        // Send notification to the specific user identified by their user ID
        await Clients.User(targetUserId).SendAsync("ReceiveNotification",
            Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, message);
    }

    public override async Task OnConnectedAsync()
    {
        // Optional: Log connection or add user to a group
        var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        // Optional: Clean up group membership
        var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }
        await base.OnDisconnectedAsync(exception);
    }
}

public class UserIdProvider : IUserIdProvider
{
    public string GetUserId(HubConnectionContext connection)
    {
        return connection.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}