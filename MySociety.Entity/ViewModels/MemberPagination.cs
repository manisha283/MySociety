namespace MySociety.Entity.ViewModels;

public class MembersPagination
{
    public List<MemberVM> Members { get; set; } = new List<MemberVM>();
    public Pagination Page { get; set; } = new Pagination();
}
