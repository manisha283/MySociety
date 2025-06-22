
namespace MySociety.Entity.ViewModels;

public class VisitorPagination
{
    public List<VisitorInfoVM> Visitors { get; set; } = new List<VisitorInfoVM>();
    public Pagination Page { get; set; } = new Pagination();
}
