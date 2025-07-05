using System.ComponentModel.DataAnnotations;
using MySociety.Entity.Models;

namespace MySociety.Entity.ViewModels;

public class NoticeVM
{
    public int Id { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
    public string Title { get; set; } = "";

    [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
    public string Description { get; set; } = "";

    [Required(ErrorMessage = "Category is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Category is required")]
    public int CategoryId { get; set; } = -1;
    public List<NoticeCategory> Categories { get; set; } = new List<NoticeCategory>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public NoticeAudienceVM AudiencesVM { get; set; } = new();
}