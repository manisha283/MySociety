﻿using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class HouseMapping
{
    public int Id { get; set; }

    public int BlockId { get; set; }

    public int FloorId { get; set; }

    public int HouseId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public virtual Block Block { get; set; } = null!;

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual Floor Floor { get; set; } = null!;

    public virtual House House { get; set; } = null!;

    public virtual User UpdatedByNavigation { get; set; } = null!;

    public virtual ICollection<UserHouseMapping> UserHouseMappings { get; set; } = new List<UserHouseMapping>();
}
