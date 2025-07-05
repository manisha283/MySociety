using MySociety.Entity.Models;
using MySociety.Service.Implementations;

namespace MySociety.Service.Interfaces;

public interface IFloorService
{
    Task<IEnumerable<Floor>> List();
    Task<IEnumerable<Floor>> List(int blockId);
    Task<int> GetNumberOfHouse(int blockId);
    Task<string> GetName(int blockId);
}
