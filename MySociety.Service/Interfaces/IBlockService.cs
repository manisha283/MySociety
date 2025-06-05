using MySociety.Entity.Models;

namespace MySociety.Service.Interfaces;

public interface IBlockService
{
    Task<IEnumerable<Block>> Get();
}
