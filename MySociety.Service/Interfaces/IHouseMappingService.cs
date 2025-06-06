namespace MySociety.Service.Interfaces;

public interface IHouseMappingService
{
    Task<int> Get(int blockId, int floorId, int houseId);
}
