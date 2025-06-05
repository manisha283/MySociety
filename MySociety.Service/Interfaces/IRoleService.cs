using MySociety.Entity.Models;

namespace MySociety.Service.Interfaces;

public interface IRoleService
{
    Task<string> Get(int id);
    IEnumerable<Role> List();
}
