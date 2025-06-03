using Application.Abstractions.Authentication;
using Domain.Users;
using Infrastructure.Authentication.Permissions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Authentication.Jwt;

internal sealed class JwtTokenProvider(
    IOptions<JwtSettings> options,
    IPermissionService permissionService) : IJwtTokenProvider
{
    private readonly JwtSettings _jwtSettings = options.Value;
    private readonly IPermissionService _permissionService = permissionService;

    public async Task<string> GenerateAsync(User user)
    {
        string secretKey = _jwtSettings.Secret;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var permissions = await _permissionService.GetPermissionsAsync(user.Id);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.Value)
        };

        foreach (var permission in permissions)
        {
            claims.Add(new(CustomClaims.Permissions, permission));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var handler = new JsonWebTokenHandler();

        return handler.CreateToken(tokenDescriptor);
    }
}
