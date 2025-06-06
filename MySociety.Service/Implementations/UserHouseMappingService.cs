using MySociety.Entity.Models;
using MySociety.Repository.Interfaces;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class UserHouseMappingService : IUserHouseMappingService
{
    private readonly IGenericRepository<UserHouseMapping> _userHouseMappingRepository;

    public UserHouseMappingService(IGenericRepository<UserHouseMapping> userHouseMappingRepository)
    {
        _userHouseMappingRepository = userHouseMappingRepository;
    }

    public async Task Add(int userId, int houseMappingId)
    {
        UserHouseMapping mapping = new()
        {
            UserId = userId,
            HouseMappingId = houseMappingId,
            CreatedBy = 1
        };

        await _userHouseMappingRepository.AddAsync(mapping);
    }

}
