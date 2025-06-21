using SharedKernal.Abstraction;

namespace SharedKernal.Abstractions;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent domainEvent, CancellationToken cancellationToken = default)
        where TEvent : IDomainEvent;
}
