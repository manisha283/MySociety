using System.Threading.Tasks;
using MySociety.Entity.Models;
using MySociety.Repository.Interfaces;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class HouseService : IHouseService
{
    private readonly IGenericRepository<House> _houseRepository;

    public HouseService(IGenericRepository<House> houseRepository)
    {
        _houseRepository = houseRepository;
    }

    public async Task<IEnumerable<House>> Get(int blockId, int floorId)
    {
        IEnumerable<House> houses = await _houseRepository.GetByCondition(
            h => h.BlockId == blockId && h.FloorId == floorId && h.DeletedBy == null
        );
        
        return houses;
    }

}
