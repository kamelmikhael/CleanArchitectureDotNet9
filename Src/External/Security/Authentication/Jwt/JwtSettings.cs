using System.ComponentModel.DataAnnotations;

namespace Security.Authentication;

public sealed class JwtSettings
{
    [Required]
    public string Issuer { get; set; }

    [Required]
    public string Audience { get; set; }

    [Required]
    public string Secret { get; set; }

    [Range(1, 120)]
    public double ExpirationInMinutes { get; set; }
}
