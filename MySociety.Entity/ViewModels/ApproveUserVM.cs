namespace MySociety.Entity.ViewModels;

public class ApproveUserVM
{
    public string Email { get; set; } = "";
    public string Name { get; set; } = "";
    public string Block { get; set; } = "";
    public string Floor { get; set; } = "";
    public string House { get; set; } = "";
    public bool IsApproved { get; set; }
}
