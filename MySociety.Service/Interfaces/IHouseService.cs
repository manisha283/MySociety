using MySociety.Entity.Models;

namespace MySociety.Service.Interfaces;

public interface IHouseService
{
    Task<IEnumerable<House>> List();
    Task<IEnumerable<House>> List(int floorId);
    Task<string> GetName(int floorId);
}
