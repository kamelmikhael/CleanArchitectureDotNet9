using Domain.Users;
using SharedKernal.Abstraction;

namespace Application.Users.Register;

internal sealed class UserRegisteredDomainEventHandler : IDomainEventHandler<UserRegisteredDomainEvent>
{
    public Task Handle(UserRegisteredDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        // TODO: Send an email verification link, etc.
        Console.WriteLine($"User registered with ID: {domainEvent.UserId}");
        return Task.CompletedTask;
    }
}
