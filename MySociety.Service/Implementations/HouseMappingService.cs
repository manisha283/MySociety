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

    public HouseMappingService(IGenericRepository<HouseMapping> houseMappingRepository)
    {
        _houseMappingRepository = houseMappingRepository;
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

        addressVM.BlockName = mapping.Block.Name;
        addressVM.BlockName = mapping.Floor.Name;
        addressVM.BlockName = mapping.House.Name;
        addressVM.UnitName = mapping.Block.Name + "-" + mapping.Floor.FloorNumber + "0" + mapping.House.HouseNumber;

        return addressVM;
    }

    public async Task<int> GetId(int userId)
    {
        Expression<Func<HouseMapping, bool>> predicate = m => m.TenantId == userId || m.OwnerId == userId;
        
        HouseMapping mapping = await _houseMappingRepository.GetByStringAsync(predicate)
                            ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "House Mapping"));

        return mapping.Id;
    }

}
