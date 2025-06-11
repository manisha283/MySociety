namespace MySociety.Entity.ViewModels;

public class ApprovalPaginationVM
{
    public List<ApproveUserVM> Users { get; set; } = new List<ApproveUserVM>();
    public Pagination Page { get; set; } = new Pagination();
}
