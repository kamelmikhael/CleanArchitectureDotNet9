using SharedKernal.Abstraction;

namespace SharedKernal.Abstractions;

public interface IEventPublisher
{
    Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken);
}
