
namespace MySociety.Entity.ViewModels;

public class VisitorPagination
{
    public List<VisitorVM> Visitors { get; set; } = new List<VisitorVM>();
    public Pagination Page { get; set; } = new Pagination();
}
