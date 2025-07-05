using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class NotificationCategory
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
}
