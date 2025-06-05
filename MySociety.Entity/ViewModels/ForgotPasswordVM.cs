using System.ComponentModel.DataAnnotations;

namespace MySociety.Entity.ViewModels;

public class ForgotPasswordVM
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
    [EmailAddress]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Format")]
    public string Email { get; set; } = null!;
}
