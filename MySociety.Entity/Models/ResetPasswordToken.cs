using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class ResetPasswordToken
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Token { get; set; } = null!;

    public bool IsUsed { get; set; }

    public DateTime Expirytime { get; set; }
}
