using System.ComponentModel.DataAnnotations;

namespace MySociety.Entity.ViewModels;

public class ChangePasswordVM
{
    public string? Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Current Password is required")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; } = "";

        [Required(AllowEmptyStrings = false, ErrorMessage = "New Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Password must contain at least one number and one uppercase and lowercase letter, and at least 8 or more characters")]
        public string NewPassword { get; set; }= "";

        [Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }= "";
}
