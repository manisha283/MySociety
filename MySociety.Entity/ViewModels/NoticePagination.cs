namespace MySociety.Entity.ViewModels;

public class NoticePagination
{
    public List<NoticeVM> Notices { get; set; } = new List<NoticeVM>();
    public Pagination Page { get; set; } = new Pagination();
}
