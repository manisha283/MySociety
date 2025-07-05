using MySociety.Entity.Models;
using MySociety.Repository.Interfaces;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class RoleService : IRoleService
{
    private readonly IGenericRepository<Role> _roleRepository;

    public RoleService(IGenericRepository<Role> roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<string> Get(int id)
    {
        Role role = await _roleRepository.GetByIdAsync(id) ?? new();
        return role.Name;
    }

    public async Task<int> Get(string name)
    {
        Role role = await _roleRepository.GetByStringAsync(r => r.Name == name) ?? new();
        return role.Id;
    }

    public IEnumerable<Role> List()
    {
        IEnumerable<Role> list = _roleRepository.GetAll();
        return list;
    }
}
