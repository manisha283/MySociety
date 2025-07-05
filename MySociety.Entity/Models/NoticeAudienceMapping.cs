using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class NoticeAudienceMapping
{
    public int Id { get; set; }

    public int? NoticeId { get; set; }

    public int GroupTypeId { get; set; }

    public int? ReferenceId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual AudienceGroupType GroupType { get; set; } = null!;

    public virtual Notice? Notice { get; set; }
}
