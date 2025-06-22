using System.ComponentModel.DataAnnotations;
using MySociety.Entity.Models;

namespace MySociety.Entity.ViewModels;

public class VehicleVM
{
    public int Id { get; set; } = 0;
    public int UserId { get; set; } = 0;

    [Required(ErrorMessage = "Number is required")]
    [RegularExpression(@"^[A-Z]{2}[0-9]{2}[A-Z]{2}[0-9]{4}$", ErrorMessage = "Vehicle number format should be like GJ01AA0001")]
    [StringLength(10, ErrorMessage = "Number cannot exceed 10 characters")]
    public string Number { get; set; } = "";

    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string Name { get; set; } = "";

    [Required(ErrorMessage = "Vehicle type is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Type is required")]
    public int TypeId { get; set; } = 0;
    public List<VehicleType> Types { get; set; } = new List<VehicleType>();
    public string? TypeName { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Parking slot number cannot be less than 0")]
    public int? ParkingSlotNo { get; set; }
}
