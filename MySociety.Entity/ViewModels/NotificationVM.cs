namespace MySociety.Entity.ViewModels;

public class NotificationVM
{
    public int Id { get; set; }
    public string Message { get; set; } = "";
    public DateTime Time { get; set; }
    public string? ActionTable { get; set; }
    public int? ActionId { get; set; }
    public string? ActionUrl { get; set; } = "http://localhost:5112/Home/Index";
    public DateTime? ReadAt { get; set; }
}