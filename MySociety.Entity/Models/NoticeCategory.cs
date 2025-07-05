using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class NoticeCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Notice> Notices { get; set; } = new List<Notice>();
}
