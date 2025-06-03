using SharedKernal.Abstraction;
using SharedKernal.Abstractions;

namespace Infrastructure.Services;

internal sealed class EventPublisher : IEventPublisher
{
    public Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Event published: {domainEvent.GetType().Name} at {DateTime.UtcNow}");

        return Task.CompletedTask;
    }
}
