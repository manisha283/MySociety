namespace MySociety.Entity.ViewModels;

public class ResponseVM
{
    public int Id { get; set; } = 0;
    public bool Success { get; set; }
    public string Message { get; set; } = "";
    public string Role { get; set; } = "";
}
