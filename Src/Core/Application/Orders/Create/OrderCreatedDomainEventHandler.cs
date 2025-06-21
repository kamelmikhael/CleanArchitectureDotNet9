using Domain.Orders;
using Rebus.Bus;
using SharedKernal.Abstraction;

namespace Application.Orders.Create;

public sealed class OrderCreatedDomainEventHandler(
    //IBus bus
    ) : IDomainEventHandler<OrderCreatedDomainEvent>
{
    public Task HandleAsync(OrderCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        //await bus.Send(new OrderCreatedEvent(domainEvent.OrderId.Value));

        return Task.CompletedTask;
    }
}
