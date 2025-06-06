using MySociety.Entity.Models;
using MySociety.Repository.Interfaces;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class HouseMappingService : IHouseMappingService
{
    private readonly IGenericRepository<HouseMapping> _houseMappingRepository;

    public HouseMappingService(IGenericRepository<HouseMapping> houseMappingRepository)
    {
        _houseMappingRepository = houseMappingRepository;
    }

    public async Task<int> Get(int blockId, int floorId, int houseId)
    {
        HouseMapping? mapping = await _houseMappingRepository.GetByStringAsync(
            m => m.BlockId == blockId && m.FloorId == floorId && m.HouseId == houseId
        );

        if (mapping == null)
        {
            return 0;
        }
        else
        {
            return mapping.Id;
        }
    }

}
