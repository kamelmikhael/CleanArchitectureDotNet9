namespace Infrastructure.Authentication;

public sealed class JwtSettings
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public string Secret { get; set; }
    public double ExpirationInMinutes { get; set; }
}
