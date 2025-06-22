using MySociety.Entity.ViewModels;

namespace MySociety.Service.Interfaces;

public interface IProfileService
{
    Task<ProfileVM> GetProfile();
}
