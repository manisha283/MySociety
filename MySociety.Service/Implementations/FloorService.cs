using MySociety.Entity.HelperModels;
using MySociety.Entity.Models;
using MySociety.Repository.Interfaces;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class FloorService : IFloorService
{
    private readonly IGenericRepository<Floor> _floorRepository;
    private readonly IBlockService _blockService;

    public FloorService(IGenericRepository<Floor> floorRepository, IBlockService blockService)
    {
        _floorRepository = floorRepository;
        _blockService = blockService;

    }

    public async Task<IEnumerable<Floor>> List(int blockId)
    {
        int noOfFloors = await _blockService.GetNumberOfFloors(blockId);

        DbResult<Floor> result = await _floorRepository.GetRecords(
            predicate: f => f.DeletedBy == null && f.FloorNumber <= noOfFloors
        );

        return result.Records;
    }

    public async Task<IEnumerable<Floor>> List()
    {

        DbResult<Floor> result = await _floorRepository.GetRecords(
            predicate: f => f.DeletedBy == null,
            orderBy: q => q.OrderBy(f => f.FloorNumber)
        );

        return result.Records;
    }

    public async Task<int> GetNumberOfHouse(int blockId)
    {
        Floor? floor = await _floorRepository.GetByIdAsync(blockId);
        if (floor == null)
        {
            return 0;
        }
        else
        {
            return floor.NoOfHouse;
        }
    }

    public async Task<string> GetName(int blockId)
    {
        Floor floor = await _floorRepository.GetByIdAsync(blockId) ?? new();
        return floor.Name;
    }

}
