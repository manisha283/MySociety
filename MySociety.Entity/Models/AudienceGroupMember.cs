using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class AudienceGroupMember
{
    public int Id { get; set; }

    public int AudienceGroupId { get; set; }

    public int MemberId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public virtual AudienceGroup AudienceGroup { get; set; } = null!;

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User Member { get; set; } = null!;
}
