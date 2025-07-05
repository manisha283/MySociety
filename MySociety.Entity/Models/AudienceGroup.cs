using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class AudienceGroup
{
    public int Id { get; set; }

    public string GroupName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public virtual ICollection<AudienceGroupMember> AudienceGroupMembers { get; set; } = new List<AudienceGroupMember>();

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User UpdatedByNavigation { get; set; } = null!;
}
