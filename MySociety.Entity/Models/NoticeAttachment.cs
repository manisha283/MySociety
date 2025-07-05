using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class NoticeAttachment
{
    public int Id { get; set; }

    public int NoticeId { get; set; }

    public string Name { get; set; } = null!;

    public int Path { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Notice Notice { get; set; } = null!;
}
