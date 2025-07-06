using MySociety.Entity.Models;
using MySociety.Repository.Interfaces;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class AudienceGroupService : IAudienceGroupService
{
    private readonly IGenericRepository<AudienceGroup> _audienceGroupRepository;

    public AudienceGroupService(IGenericRepository<AudienceGroup> audienceGroupRepository)
    {
        _audienceGroupRepository = audienceGroupRepository;
    }

    public async Task<IEnumerable<AudienceGroup>> List()
    {
        var dbResult = await _audienceGroupRepository.GetRecords
        (
            predicate: g => g.DeletedBy == null,
            orderBy: q => q.OrderBy(g => g.GroupName)
        );

        return dbResult.Records;
    }

}
