namespace MySociety.Entity.ViewModels;

public class NotifyVM
{
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public int TargetId { get; set; }
    public bool Status { get; set; }
    public string Message { get; set; } = "";
}
