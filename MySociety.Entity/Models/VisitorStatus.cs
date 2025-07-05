using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class VisitorStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Visitor> Visitors { get; set; } = new List<Visitor>();
}
