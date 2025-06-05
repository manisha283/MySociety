using System.Security.Claims;

namespace MySociety.Service.Interfaces;

public interface IJwtService
{
    Task<string> GenerateToken(string email);
}

