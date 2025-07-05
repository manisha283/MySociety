using MySociety.Entity.Models;

namespace MySociety.Entity.ViewModels;

public class MemberIndexVM
{
    public IEnumerable<Role> Roles { get; set; } = new List<Role>();

    public AddressVM Address { get; set; } = new();
}
