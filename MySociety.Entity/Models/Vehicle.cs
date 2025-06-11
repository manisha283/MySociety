using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class Vehicle
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? VehicleNumber { get; set; }

    public string Name { get; set; } = null!;

    public int VehicleTypeId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual User UpdatedByNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual VehicleType VehicleType { get; set; } = null!;
}
