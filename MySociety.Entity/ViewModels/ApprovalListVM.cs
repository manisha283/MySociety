namespace MySociety.Entity.ViewModels;

public class ApprovalListVM
{
    public ApproveUserVM User { get; set; } = new ApproveUserVM();
    public Pagination Page { get; set; } = new Pagination();
}
