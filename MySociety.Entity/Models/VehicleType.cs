using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class VehicleType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
