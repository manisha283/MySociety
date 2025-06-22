namespace MySociety.Entity.ViewModels;

public class VehiclePagination
{
    public List<VehicleVM> Vehicles { get; set; } = new List<VehicleVM>();
    public Pagination Page { get; set; } = new Pagination();
}
