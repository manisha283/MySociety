using System.ComponentModel.DataAnnotations;

namespace MySociety.Entity.ViewModels;

public class LoginVM
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
    [EmailAddress]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Format")]
    public required string Email { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; } = false;
}
