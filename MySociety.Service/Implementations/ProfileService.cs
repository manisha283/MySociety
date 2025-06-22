using Microsoft.EntityFrameworkCore;
using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;
using MySociety.Repository.Interfaces;
using MySociety.Service.Common;
using MySociety.Service.Exceptions;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class ProfileService : IProfileService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IHttpService _httpService;

    public ProfileService(IGenericRepository<User> userRepository, IHttpService httpService)
    {
        _userRepository = userRepository;
        _httpService = httpService;
    }


    public async Task<ProfileVM> GetProfile()
    {
        int userId = await _httpService.LoggedInUserId();

        User user = await _userRepository.GetByStringAsync(
            predicate: u => u.Id == userId,
            queries: new List<Func<IQueryable<User>, IQueryable<User>>>
            {
                q => q.Include(u => u.HouseMappingOwners)
                    .ThenInclude(m => m.Block),
                q => q.Include(u => u.HouseMappingOwners)
                    .ThenInclude(uhm => uhm.Floor),
                q => q.Include(u => u.HouseMappingOwners)
                    .ThenInclude(uhm => uhm.House)
            }
        ) ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}", "User"));

        ProfileVM profile = new()
        {
            UserId = userId,
            Name = user.Name,
            Phone = user.Phone,
            Email = user.Email,
            ProfileImageUrl = user.ProfileImg,
            Block = user.HouseMappingOwners.Select(m => m.Block.Name).FirstOrDefault(),
            Floor = user.HouseMappingOwners.Select(m => m.Floor.Name).FirstOrDefault(),
            House = user.HouseMappingOwners.Select(m => m.House.Name).FirstOrDefault()
        };

        return profile;
    }
}
