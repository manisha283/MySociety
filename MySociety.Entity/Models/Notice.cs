using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class Notice
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int NoticeCategoryId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<NoticeAttachment> NoticeAttachments { get; set; } = new List<NoticeAttachment>();

    public virtual ICollection<NoticeAudienceMapping> NoticeAudienceMappings { get; set; } = new List<NoticeAudienceMapping>();

    public virtual NoticeCategory NoticeCategory { get; set; } = null!;

    public virtual User UpdatedByNavigation { get; set; } = null!;
}
