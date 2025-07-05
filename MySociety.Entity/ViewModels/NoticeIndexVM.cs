using MySociety.Entity.Models;

namespace MySociety.Entity.ViewModels;

public class NoticeIndexVM
{
    public List<NoticeCategory> Categories { get; set; } = new List<NoticeCategory>();
    public List<AudienceGroupType> Audiences { get; set; } = new List<AudienceGroupType>();
    public List<int> SelectedGroup { get; set; } = new List<int>();
    public NoticeAudienceVM SelectAudienceVM { get; set; } = new();
}
