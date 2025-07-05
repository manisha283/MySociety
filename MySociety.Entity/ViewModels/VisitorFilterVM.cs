namespace MySociety.Entity.ViewModels;

public class VisitorFilterVM : FilterVM
{
    public string Role { get; set; } = "";
    public int VisitorStatus { get; set; }
    public string CheckOutStatus { get; set; } = "";
    public int VisitPurpose { get; set; } = 0;
}
