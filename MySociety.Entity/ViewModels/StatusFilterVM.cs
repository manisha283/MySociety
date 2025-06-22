namespace MySociety.Entity.ViewModels;

public class StatusFilterVM : FilterVM
{
    public string Role { get; set; } = "";
    public string Status { get; set; } = "";
    public string DateRange { get; set; } = "";
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
    public string CheckOutStatus { get; set; } = "";
    public int VisitPurpose { get; set; } = 0;
}
