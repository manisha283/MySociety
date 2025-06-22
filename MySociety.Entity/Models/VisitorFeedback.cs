using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class VisitorFeedback
{
    public int Id { get; set; }

    public int VisitorId { get; set; }

    public int? Rating { get; set; }

    public string? Feedback { get; set; }

    public virtual Visitor Visitor { get; set; } = null!;
}
