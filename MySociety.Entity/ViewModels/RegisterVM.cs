using System.ComponentModel.DataAnnotations;
using MySociety.Entity.Models;

namespace MySociety.Entity.ViewModels;

public class RegisterVM
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email Format")]
    public string Email { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
    public string Name { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "New Password is required")]
    [DataType(DataType.Password)]
    [RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Password must contain at least one number and one uppercase and lowercase letter, and at least 8 or more characters")]
    public string Password { get; set; } = "";

    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; } = "";

    [Required(ErrorMessage = "Role is required")]
    public int RoleId { get; set; } = 0;
    public IEnumerable<Role> Roles { get; set; } = new List<Role>();

    public int BlockId { get; set; } = 0;
    public IEnumerable<Block> Blocks { get; set; } = new List<Block>();

    public int FloorId { get; set; } = 0;
    public IEnumerable<Floor> Floors { get; set; } = new List<Floor>();

    public int HouseId { get; set; } = 0;
    public IEnumerable<House> Houses { get; set; } = new List<House>();

    public string BlockName { get; set; } = "";
    public string FloorName { get; set; } = "";
    public string HouseName { get; set; } = "";
}
