using MySociety.Entity.Models;

namespace MySociety.Service.Interfaces;

public interface IBlockService
{
    Task<IEnumerable<Block>> List();
    Task<int> GetNumberOfFloors(int blockId);
    Task<string> GetName(int blockId);
}
