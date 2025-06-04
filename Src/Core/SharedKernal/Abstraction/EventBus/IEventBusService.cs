namespace SharedKernal.Abstraction.EventBus;

public interface IEventBusService
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : class;
}
