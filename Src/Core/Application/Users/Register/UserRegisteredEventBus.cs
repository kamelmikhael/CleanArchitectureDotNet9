namespace Application.Users.Register;

public record UserRegisteredEventBus
{
    public Guid Id { get; init; }

    public string Username { get; set; } = string.Empty;
}
