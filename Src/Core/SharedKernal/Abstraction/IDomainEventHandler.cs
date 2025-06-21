namespace SharedKernal.Abstraction;

public interface IDomainEventHandler<in T> where T : IDomainEvent
{
    Task HandleAsync(T domainEvent, CancellationToken cancellationToken);
}
