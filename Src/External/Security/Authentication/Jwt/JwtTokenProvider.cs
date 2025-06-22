using Application.Abstractions.Authentication;
using Domain.Users;
using Security.Authentication.Permissions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace Security.Authentication.Jwt;

internal sealed class JwtTokenProvider(
    JwtSettings jwtSettings,
    IPermissionService permissionService) : IJwtTokenProvider
{
    public async Task<string> GenerateAsync(User user)
    {
        string secretKey = jwtSettings.Secret;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var permissions = await permissionService.GetPermissionsAsync(user.Id);

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
            Expires = DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience
        };

        var handler = new JsonWebTokenHandler();

        return handler.CreateToken(tokenDescriptor);
    }
}
