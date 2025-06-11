namespace MySociety.Entity.ViewModels;

public class FilterVM
{
    public int Id { get; set; } = 0;
    public int PageSize { get; set; } = 5;
    public int PageNumber { get; set; } = 1;
    public string Column { get; set; } = "";
    public string Sort { get; set; } = "";
    public string Search { get; set; } = "";
}
