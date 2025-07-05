namespace MySociety.Entity.ViewModels;

public class MemberVM
{
    public int Id { get; set; } = 0;
    public string Email { get; set; } = "";
    public string Name { get; set; } = "";
    public AddressVM Address { get; set; } = new();
    public string Role { get; set; } = "";
    public bool? IsApproved { get; set; }
    public DateTime RegisteredAt { get; set; }
}
