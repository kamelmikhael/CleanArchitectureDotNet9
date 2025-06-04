using System.ComponentModel.DataAnnotations;

namespace MessageBroker;

public class MessageBrokerSettings
{
    [Required]
    public string Host { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
