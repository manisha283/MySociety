using System.Threading.Tasks;
using MySociety.Entity.HelperModels;
using MySociety.Entity.Models;
using MySociety.Repository.Interfaces;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class BlockService : IBlockService
{
    private readonly IGenericRepository<Block> _blockRepository;

    public BlockService(IGenericRepository<Block> blockRepository)
    {
        _blockRepository = blockRepository;
    }

    public async Task<IEnumerable<Block>> List()
    {
        DbResult<Block> result = await _blockRepository.GetRecords(
            b => b.DeletedBy == null
        );

        return result.Records;
    }

    public async Task<int> GetNumberOfFloors(int blockId)
    {
        Block? block = await _blockRepository.GetByIdAsync(blockId);
        if (block == null)
        {
            return 0;
        }
        else
        {
            return block.NoOfFloor;
        }
    }

    public async Task<string> GetName(int blockId)
    {
        Block? block = await _blockRepository.GetByIdAsync(blockId);
        return block.Name;
    }

}
