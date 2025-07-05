using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class Visitor
{
    public int Id { get; set; }

    public int HouseMappingId { get; set; }

    public string Name { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int VisitPurposeId { get; set; }

    public int NoOfVisitors { get; set; }

    public DateTime? CheckInTime { get; set; }

    public DateTime? CheckOutTime { get; set; }

    public string? VehicleNo { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdatedAt { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedBy { get; set; }

    public string? VisitPurposeReason { get; set; }

    public int StatusId { get; set; }

    public virtual HouseMapping HouseMapping { get; set; } = null!;

    public virtual VisitorStatus Status { get; set; } = null!;

    public virtual VisitPurpose VisitPurpose { get; set; } = null!;

    public virtual ICollection<VisitorFeedback> VisitorFeedbacks { get; set; } = new List<VisitorFeedback>();
}
