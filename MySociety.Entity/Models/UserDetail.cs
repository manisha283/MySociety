using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class UserDetail
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string ProfileImg { get; set; } = null!;

    public int Phone { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
