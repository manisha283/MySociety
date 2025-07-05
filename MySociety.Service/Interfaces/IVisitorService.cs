using MySociety.Entity.Models;
using MySociety.Entity.ViewModels;

namespace MySociety.Service.Interfaces;

public interface IVisitorService
{
    Task<VisitorVM> Get(int id);
    Task<VisitorPagination> List(VisitorFilterVM filter);
    Task<ResponseVM> Save(VisitorVM visitorVM);
    Task VisitorStatus(int id, bool IsApproved);
    List<VisitPurpose> VisitPurposes();
    List<VisitorStatus> VisitorStatuses();
    Task VisitorStatusExpired(int id);
    Task<ResponseVM> CheckOut(int id, int rating, string feedback);
}
