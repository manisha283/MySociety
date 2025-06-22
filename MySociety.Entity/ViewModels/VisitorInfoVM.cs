namespace MySociety.Entity.ViewModels;

public class VisitorInfoVM
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = "";
    public string Phone { get; set; } = "";
    public string? VisitPurpose { get; set; } = "";
    public int NoOfVisitors { get; set; } = 1;
    public string? Vehicle { get; set; }
    public DateTime WaitingSince { get; set; }
    public AddressVM Address { get; set; } = new();
    public bool? IsApprove { get; set; }
    public string ApprovalStatus { get; set; } = "";
    public DateTime? CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }

}
