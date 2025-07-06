using MySociety.Entity.Models;

namespace MySociety.Service.Interfaces;

public interface INoticeAudienceMappingService
{
    Task<IEnumerable<AudienceGroupType>> AudienceGroupTypeList();
}
