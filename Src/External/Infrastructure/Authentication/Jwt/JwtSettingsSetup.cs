using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Infrastructure.Authentication.Jwt;

public class JwtSettingsSetup(IConfiguration configuration) : IConfigureOptions<JwtSettings>
{
    public void Configure(JwtSettings options)
    {
        configuration.GetSection(nameof(JwtSettings)).Bind(options);
    }
}
