using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class User
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? ProfileImg { get; set; }

    public string? Phone { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public bool? IsApproved { get; set; }

    public virtual ICollection<Block> BlockCreatedByNavigations { get; set; } = new List<Block>();

    public virtual ICollection<Block> BlockUpdatedByNavigations { get; set; } = new List<Block>();

    public virtual ICollection<Floor> FloorCreatedByNavigations { get; set; } = new List<Floor>();

    public virtual ICollection<Floor> FloorUpdatedByNavigations { get; set; } = new List<Floor>();

    public virtual ICollection<House> HouseCreatedByNavigations { get; set; } = new List<House>();

    public virtual ICollection<HouseMapping> HouseMappingCreatedByNavigations { get; set; } = new List<HouseMapping>();

    public virtual ICollection<HouseMapping> HouseMappingOwners { get; set; } = new List<HouseMapping>();

    public virtual ICollection<HouseMapping> HouseMappingTenants { get; set; } = new List<HouseMapping>();

    public virtual ICollection<HouseMapping> HouseMappingUpdatedByNavigations { get; set; } = new List<HouseMapping>();

    public virtual ICollection<House> HouseUpdatedByNavigations { get; set; } = new List<House>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<UserOtp> UserOtps { get; set; } = new List<UserOtp>();

    public virtual ICollection<Vehicle> VehicleCreatedByNavigations { get; set; } = new List<Vehicle>();

    public virtual ICollection<Vehicle> VehicleUpdatedByNavigations { get; set; } = new List<Vehicle>();

    public virtual ICollection<Vehicle> VehicleUsers { get; set; } = new List<Vehicle>();
}
