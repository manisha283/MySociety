using System;
using System.Collections.Generic;

namespace MySociety.Entity.Models;

public partial class UserOtp
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string OtpCode { get; set; } = null!;

    public DateTime ExpiryTime { get; set; }

    public bool IsUsed { get; set; }

    public virtual User User { get; set; } = null!;
}
