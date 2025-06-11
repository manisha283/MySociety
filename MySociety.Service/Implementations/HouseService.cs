using System.Threading.Tasks;
using MySociety.Entity.HelperModels;
using MySociety.Entity.Models;
using MySociety.Repository.Interfaces;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class HouseService : IHouseService
{
    private readonly IGenericRepository<House> _houseRepository;
    private readonly IFloorService _floorService;

    public HouseService(IGenericRepository<House> houseRepository, IFloorService floorService)
    {
        _houseRepository = houseRepository;
        _floorService = floorService;
    }

    public async Task<IEnumerable<House>> List(int floorId)
    {
        int NoOfHouse = await _floorService.GetNumberOfHouse(floorId);

        DbResult<House> dbResult = await _houseRepository.GetRecords(
            predicate: f => f.DeletedBy == null && f.HouseNumber <= NoOfHouse
        );

        return dbResult.Records;
    }

    public async Task<string> GetName(int floorId)
    {
        House? house = await _houseRepository.GetByIdAsync(floorId);
        return house!.Name;
    }

}
