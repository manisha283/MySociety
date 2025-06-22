namespace MySociety.Entity.ViewModels;

public class ApproveUserVM
{
    public int UserId { get; set; } = 0;
    public string Email { get; set; } = "";
    public string Name { get; set; } = "";
    public AddressVM Address { get; set; } = new();
    public string Role { get; set; } = "";
    public bool IsApproved { get; set; } = false;
}
