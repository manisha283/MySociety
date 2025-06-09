using System.ComponentModel.DataAnnotations;

namespace MySociety.Entity.ViewModels;

public class LoginVM
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
    [EmailAddress]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Format")]
    public string Email { get; set; } = "";

    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; } = false;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Otp is required")]
    [RegularExpression(@"^\d{6}$", ErrorMessage = "Enter 6 digit OTP")]
    public string OtpCode { get; set; } = "";
    public bool LoginEnable { get; set; } = false;
}
