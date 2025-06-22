using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class VisitPurpose
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Visitor> Visitors { get; set; } = new List<Visitor>();
}
