using System.ComponentModel.DataAnnotations;

namespace MySociety.Entity.ViewModels;

public class ResetPasswordVM
{
    public string Token { get; set; } = "";

    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Password must contain at least one number and one uppercase and lowercase letter, and at least 8 or more characters")]
    public string NewPassword { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password is required")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "The password and confirmation password does not match.")]
    public string ConfirmPassword { get; set; } = null!;
}
