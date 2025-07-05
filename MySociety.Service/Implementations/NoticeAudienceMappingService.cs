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

    public NoticeAudienceMappingService(IGenericRepository<NoticeAudienceMapping> mappingRepository, IHttpService httpService)
    {
        _mappingRepository = mappingRepository;
        _httpService = httpService;

    }

    // public async Task Save(NoticeVM noticeVM)
    // {
    //     DbResult<NoticeAudienceMapping> dbResult = await _mappingRepository.GetRecords(predicate: m => m.DeletedBy == null && m.NoticeId == noticeVM.Id);

    //     List<int> existingMapping = dbResult.Records.Select(r => r.Id).ToList();

    //     //Delete Mapping 
    //     List<long> removeMapping = existingMapping.Except(noticeVM.)
    // }

}
