using MySociety.Entity.Models;

namespace MySociety.Service.Interfaces;

public interface IAudienceGroupService
{
    Task<IEnumerable<AudienceGroup>> List();
}
