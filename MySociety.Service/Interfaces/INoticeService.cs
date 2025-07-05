using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;

namespace MySociety.Service.Interfaces;

public interface INoticeService
{
    Task<NoticeVM> Get(int id);
    List<AudienceGroupType> GetNoticeAudiences();
    Task<List<NoticeCategory>> GetNoticeCategories();
    Task<ResponseVM> Save(NoticeVM noticeVM);
    Task<NoticePagination> List(NoticeFilterVM filter);
    Task Delete(int id);
}
