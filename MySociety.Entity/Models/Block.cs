using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class Block
{
    public int Id { get; set; }

    public int BlockNumber { get; set; }

    public string Name { get; set; } = null!;

    public int NoOfFloor { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<HouseMapping> HouseMappings { get; set; } = new List<HouseMapping>();

    public virtual User UpdatedByNavigation { get; set; } = null!;
}
