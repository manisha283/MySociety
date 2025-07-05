using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;
using MySociety.Repository.Interfaces;
using MySociety.Service.Common;
using MySociety.Service.Exceptions;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class HouseMappingService : IHouseMappingService
{
    private readonly IGenericRepository<HouseMapping> _houseMappingRepository;
    private readonly IBlockService _blockService;
    private readonly IFloorService _floorService;
    private readonly IHouseService _houseService;

    public HouseMappingService(IGenericRepository<HouseMapping> houseMappingRepository, IBlockService blockService, IFloorService floorService, IHouseService houseService)
    {
        _houseMappingRepository = houseMappingRepository;
        _blockService = blockService;
        _floorService = floorService;
        _houseService = houseService;
    }

    public async Task<int> Get(AddressVM address)
    {
        HouseMapping? mapping = await _houseMappingRepository.GetByStringAsync(
            m => m.BlockId == address.BlockId && m.FloorId == address.FloorId && m.HouseId == address.HouseId
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

    public async Task<AddressVM> GetAddress(int id)
    {
        AddressVM addressVM = new();

        HouseMapping mapping = await _houseMappingRepository.GetByStringAsync(
            predicate: m => m.Id == id && m.DeletedBy == null,
            queries: new List<Func<IQueryable<HouseMapping>, IQueryable<HouseMapping>>>
            {
                q => q.Include(m => m.Block),
                q => q.Include(m => m.Floor),
                q => q.Include(m => m.House),
            }
        ) ?? new();

        addressVM.BlockId = mapping.BlockId;
        addressVM.FloorId = mapping.FloorId;
        addressVM.HouseId = mapping.HouseId;

        addressVM.BlockName = mapping.Block.Name;
        addressVM.FloorName = mapping.Floor.Name;
        addressVM.HouseName = mapping.House.Name;
        addressVM.UnitName = mapping.Block.Name + "-" + mapping.Floor.FloorNumber + "0" + mapping.House.HouseNumber;

        addressVM.Blocks = await _blockService.List();
        addressVM.Floors = await _floorService.List();
        addressVM.Houses = await _houseService.List();

        return addressVM;
    }

    public async Task<int> GetId(int userId)
    {
        HouseMapping mapping = await _houseMappingRepository.GetByStringAsync(
                                predicate: m => m.Users.Any(u => u.Id == userId),
                                queries: new List<Func<IQueryable<HouseMapping>, IQueryable<HouseMapping>>>
                                {
                                    q => q.Include(m => m.Users)
                                }
                            )
                            ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "House Mapping"));

        return mapping.Id;
    }

    public async Task<AddressVM> List()
    {
        AddressVM addressVM = new()
        {
            Blocks = await _blockService.List(),
            Floors = await _floorService.List(),
            Houses = await _houseService.List()
        };

        return addressVM;
    }

}
