using System.ComponentModel.DataAnnotations;
using MySociety.Entity.Models;

namespace MySociety.Entity.ViewModels;

public class VisitorVM
{
    public int Id { get; set; } = 0;
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Phone number is required")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
    public string Phone { get; set; } = "";

    public AddressVM Address { get; set; } = new();

    [Required(AllowEmptyStrings = false, ErrorMessage = "Visit Purpose is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Visit Purpose is required")]
    public int VisitPurposeId { get; set; } = -1;
    public List<VisitPurpose> VisitPurposes { get; set; } = new();

    [Required(ErrorMessage = "Reason is required")]
    public string? VisitReason { get; set; }


    [RegularExpression(@"^[A-Z]{2}[0-9]{2}[A-Z]{2}[0-9]{4}$", ErrorMessage = "Vehicle number format should be like GJ01AA0001")]
    public string? VehicleNumber { get; set; }


    [Required(ErrorMessage = "Number of visitor is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Number of visitor cannot be less than 0")]
    public int NoOfVisitors { get; set; }
}
