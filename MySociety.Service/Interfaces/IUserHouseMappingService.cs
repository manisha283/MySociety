namespace MySociety.Service.Interfaces;

public interface IUserHouseMappingService
{
    Task Add(int userId, int houseMappingId);
}
