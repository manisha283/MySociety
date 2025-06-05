using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class Floor
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public int FloorNo { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<House> Houses { get; set; } = new List<House>();

    public virtual User UpdatedByNavigation { get; set; } = null!;
}
