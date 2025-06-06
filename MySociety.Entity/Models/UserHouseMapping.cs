using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class UserHouseMapping
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int HouseMappingId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual HouseMapping HouseMapping { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
