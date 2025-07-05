namespace MySociety.Entity.ViewModels;

public class MemberFilterVM : FilterVM
{
    public int RoleId { get; set; }
    public int BlockId { get; set; }
    public int FloorId { get; set; }
    public int HouseId { get; set; }
    public string Status { get; set; } = "";
}
