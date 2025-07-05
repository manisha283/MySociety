using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class Notification
{
    public int Id { get; set; }

    public int? SenderId { get; set; }

    public int ReceiverId { get; set; }

    public string Message { get; set; } = null!;

    public DateTime? ReadAt { get; set; }

    public string? TargetEntity { get; set; }

    public int? TargetId { get; set; }

    public string? TargetUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public int? TargetEntityId { get; set; }

    public virtual User Receiver { get; set; } = null!;

    public virtual User? Sender { get; set; }
}
