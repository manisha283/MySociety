using MySociety.Entity.ViewModels;

namespace MySociety.Service.Interfaces;

public interface IVehicleService
{
    Task<VehicleVM> Get(int vehicleId);
    Task<ResponseVM> Save(VehicleVM vehicleVM);
    Task<VehiclePagination> List(FilterVM filter);
    Task Delete(int vehicleId);
}
