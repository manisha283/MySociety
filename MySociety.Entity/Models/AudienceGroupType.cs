using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class AudienceGroupType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<NoticeAudienceMapping> NoticeAudienceMappings { get; set; } = new List<NoticeAudienceMapping>();
}
