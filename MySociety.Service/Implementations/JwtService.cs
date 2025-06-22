using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MySociety.Entity.Models;
using MySociety.Repository.Interfaces;
using MySociety.Service.Common;
using MySociety.Service.Configuration;
using MySociety.Service.Exceptions;
using MySociety.Service.Interfaces;

namespace MySociety.Service.Implementations;

public class JwtService : IJwtService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IRoleService _roleService;

    public JwtService(IGenericRepository<User> userRepository, IRoleService roleService)
    {
        _userRepository = userRepository;
        _roleService = roleService;
    }

    public async Task<string> GenerateToken(string email)
    {
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(JwtConfig.Key));
        SigningCredentials? credentials = new(key, SecurityAlgorithms.HmacSha256);

        User user = await _userRepository.GetByStringAsync(u => u.Email == email) ?? throw new NotFoundException(NotificationMessages.NotFound.Replace("{0}","User"));
        string role = await _roleService.Get(user.RoleId);
        
        List<Claim>? claims = new()
        {
            new("name", user.Name),
            new("email", email),
            new("role", role),
            new("roleId", user.RoleId.ToString()),
        };

        JwtSecurityToken? token = new(
            issuer: JwtConfig.Issuer,
            audience: JwtConfig.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(JwtConfig.TokenDuration),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Extracts claims from a JWT token.
    public static ClaimsPrincipal? GetClaimsFromToken(string token)
    {
        JwtSecurityTokenHandler? handler = new();
        JwtSecurityToken? jwtToken = handler.ReadJwtToken(token);
        ClaimsIdentity? claims = new(jwtToken.Claims);
        return new ClaimsPrincipal(claims);
    }

    // Retrieves a specific claim value from a JWT token.
    public static string? GetClaimValue(string token, string claimType)
    {
        ClaimsPrincipal? claimsPrincipal = GetClaimsFromToken(token);
        string? value = claimsPrincipal?.FindFirst(claimType)?.Value;
        return value;
    }
}
