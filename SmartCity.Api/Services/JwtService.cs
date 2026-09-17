using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartCity.Api.Configuration;
using SmartCity.Api.Models;
using SmartCity.Api.Repositories;

namespace SmartCity.Api.Services;

public class JwtService
{
    private readonly JwtSettings _jwtSettings;
    private readonly RoleRepository _roleRepository;

    public JwtService(IOptions<JwtSettings> jwtSettings, RoleRepository roleRepository)
    {
        _jwtSettings = jwtSettings.Value;
        _roleRepository = roleRepository;
    }

    public async Task<string> GenerateTokenAsync(User user)
    {
        var role = await _roleRepository.GetByIdAsync(user.RoleId ?? string.Empty);
        if (role is null)
        {
            throw new InvalidOperationException("Role not found.");
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id ?? string.Empty),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Role, role.Name),
            new Claim("role_id", user.RoleId ?? string.Empty)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
