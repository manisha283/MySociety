using MySociety.Entity.Models;

namespace MySociety.Entity.ViewModels;

public class NoticeAudienceVM
{
    public IEnumerable<Role> Roles { get; set; } = new List<Role>();
    public IEnumerable<Block> Blocks { get; set; } = new List<Block>();
    public IEnumerable<Floor> Floors { get; set; } = new List<Floor>();
    public IEnumerable<AudienceGroupType> GroupTypes { get; set; } = new List<AudienceGroupType>();
    public IEnumerable<AudienceGroup> CustomAudienceGroups { get; set; } = new List<AudienceGroup>();
    public List<MemberVM> Members { get; set; } = new List<MemberVM>();
    public List<NoticeAudienceMappingVM> SelectedAudience { get; set; } = new List<NoticeAudienceMappingVM>();
}
