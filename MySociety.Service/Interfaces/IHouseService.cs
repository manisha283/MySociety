using MySociety.Entity.Models;

namespace MySociety.Service.Interfaces;

public interface IHouseService
{
    Task<IEnumerable<House>> Get(int blockId, int floorId);
}
