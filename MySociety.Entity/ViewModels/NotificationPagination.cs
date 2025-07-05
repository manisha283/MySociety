namespace MySociety.Entity.ViewModels;

public class NotificationPagination
{
    public List<NotificationVM> Notifications { get; set; } = new List<NotificationVM>();
    public Pagination Page { get; set; } = new Pagination();
}
