using MySociety.Entity.HelperModels;
using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;
using MySociety.Repository.Interfaces;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class NoticeAudienceMappingService : INoticeAudienceMappingService
{
    private readonly IGenericRepository<NoticeAudienceMapping> _mappingRepository;
    private readonly IHttpService _httpService;
    private readonly IGenericRepository<AudienceGroupType> _audienceGroupTypeRepository;

    public NoticeAudienceMappingService(IGenericRepository<NoticeAudienceMapping> mappingRepository, IHttpService httpService, IGenericRepository<AudienceGroupType> audienceGroupTypeRepository)
    {
        _mappingRepository = mappingRepository;
        _httpService = httpService;
        _audienceGroupTypeRepository = audienceGroupTypeRepository;
    }

    public async Task<IEnumerable<AudienceGroupType>> AudienceGroupTypeList()
    {
        var dbResult = await _audienceGroupTypeRepository.GetRecords
        (
            orderBy: q => q.OrderBy(g => g.Id)
        );

        return dbResult.Records;
    }

    // public async Task Save(NoticeVM noticeVM)
    // {
    //     DbResult<NoticeAudienceMapping> dbResult = await _mappingRepository.GetRecords(predicate: m => m.DeletedBy == null && m.NoticeId == noticeVM.Id);

    //     List<int> existingMapping = dbResult.Records.Select(r => r.Id).ToList();

    //     //Delete Mapping 
    //     List<long> removeMapping = existingMapping.Except(noticeVM.)
    // }

}
