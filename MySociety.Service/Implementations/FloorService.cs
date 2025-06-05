using MySociety.Entity.Models;
using MySociety.Repository.Interfaces;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class FloorService : IFloorService
{
    private readonly IGenericRepository<Floor> _floorRepository;

    public FloorService(IGenericRepository<Floor> floorRepository)
    {
        _floorRepository = floorRepository;
    }

    public async Task<IEnumerable<Floor>> Get(int num)
    {
        IEnumerable<Floor> list = await _floorRepository.GetByCondition(
            predicate: f => f.DeletedBy == null && f.FloorNo <= num
        );
        
        return list;
    }

}
