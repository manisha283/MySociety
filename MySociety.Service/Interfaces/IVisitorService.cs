using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;

namespace MySociety.Service.Interfaces;

public interface IVisitorService
{
    Task<VisitorVM> Get(int id);
    Task<VisitorPagination> List(StatusFilterVM filter);
    Task<ResponseVM> Save(VisitorVM visitorVM);
    Task<ResponseVM> VisitorStatus(int id, bool IsApproved);
    List<VisitPurpose> VisitPurposes();
    Task CheckOut(int id, int rating, string feedback);
}
