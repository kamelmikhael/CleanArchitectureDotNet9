using MassTransit;
using SharedKernal.Abstraction.EventBus;

namespace MessageBroker;

public sealed class EventBusService(
    IPublishEndpoint publishEndpoint) : IEventBusService
{
    public Task PublishAsync<TEvent>(
        TEvent @event, 
        CancellationToken cancellationToken = default) where TEvent : class
        => publishEndpoint.Publish(@event, cancellationToken);
}
