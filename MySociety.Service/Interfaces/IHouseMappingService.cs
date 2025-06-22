using MySociety.Entity.ViewModels;

namespace MySociety.Service.Interfaces;

public interface IHouseMappingService
{
    Task<int> Get(AddressVM address);
    Task<AddressVM> GetAddress(int id);
    Task<int> GetId(int userId);
}
