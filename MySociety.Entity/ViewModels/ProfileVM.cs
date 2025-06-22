using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using MySociety.Entity.Attributes;

namespace MySociety.Entity.ViewModels;

public class ProfileVM
{
    public int UserId { get; set; } = 0;

    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
    public string? Phone { get; set; }

    public string Email { get; set; } = "";

    public string? ProfileImageUrl { get; set; }

    [ImageType]
    public IFormFile? Image { get; set; } = null!;

    public string? Role { get; set; } = null!;

    public string? Block { get; set; } = "";
    public string? Floor { get; set; } = "";
    public string? House { get; set; } = "";
}
