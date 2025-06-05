using MySociety.Entity.Models;
using MySociety.Service.Implementations;

namespace MySociety.Service.Interfaces;

public interface IFloorService
{
    Task<IEnumerable<Floor>> Get(int num);
}
