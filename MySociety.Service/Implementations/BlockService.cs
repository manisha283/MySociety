using System.Threading.Tasks;
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

    public async Task<IEnumerable<Block>> Get()
    {
        IEnumerable<Block> list = await _blockRepository.GetByCondition(
            b => b.DeletedBy == null
        );

        return list;
    }

}
