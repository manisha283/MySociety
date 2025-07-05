namespace MySociety.Service.Interfaces;

public interface INotificationHubService
{
    Task NotifyUser(int userId, string message);
}
