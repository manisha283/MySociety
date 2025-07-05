using System.ComponentModel.DataAnnotations;
using MySociety.Entity.Models;

namespace MySociety.Entity.ViewModels;

public class AddressVM
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Block is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Block is required")]
    public int BlockId { get; set; } = -1;
    public string? BlockName { get; set; }
    public IEnumerable<Block> Blocks { get; set; } = new List<Block>();


    [Required(AllowEmptyStrings = false, ErrorMessage = "Floor is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Floor is required")]
    public int FloorId { get; set; } = -1;
    public string? FloorName { get; set; }
    public IEnumerable<Floor> Floors { get; set; } = new List<Floor>();


    [Required(AllowEmptyStrings = false, ErrorMessage = "House is required")]
    [Range(1, int.MaxValue, ErrorMessage = "House is required")]
    public int HouseId { get; set; } = -1;
    public string? HouseName { get; set; }
    public IEnumerable<House> Houses { get; set; } = new List<House>();

    public string? UnitName { get; set; } = "";
    public int UnitId { get; set; } = 0;

}
