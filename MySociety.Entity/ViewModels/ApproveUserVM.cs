namespace MySociety.Entity.ViewModels;

public class ApproveUserVM
{
    public int UserId { get; set; } = 0;
    public string Email { get; set; } = "";
    public string Name { get; set; } = "";
    public string Block { get; set; } = "";
    public string Floor { get; set; } = "";
    public string House { get; set; } = "";
    public string Role { get; set; } = "";
    public bool IsApproved { get; set; } = false;
}
