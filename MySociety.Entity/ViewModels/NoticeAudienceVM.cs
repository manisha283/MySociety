using MySociety.Entity.Models;

namespace MySociety.Entity.ViewModels;

public class NoticeAudienceVM
{
    public IEnumerable<Role> Roles { get; set; } = new List<Role>();
    public IEnumerable<Block> Blocks { get; set; } = new List<Block>();
    public IEnumerable<Floor> Floors { get; set; } = new List<Floor>();
    public List<AudienceGroupType> GroupTypes { get; set; } = new List<AudienceGroupType>();
    public List<AudienceGroup> AudienceGroups { get; set; } = new List<AudienceGroup>();
    public List<User> Members { get; set; } = new List<User>();
}
