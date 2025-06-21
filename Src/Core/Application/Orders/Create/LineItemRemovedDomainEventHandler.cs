using Domain.Orders;
using SharedKernal.Abstraction;

namespace Application.Orders.Create;

public sealed class LineItemRemovedDomainEventHandler : IDomainEventHandler<LineItemRemovedDomainEvent>
{
    public Task HandleAsync(LineItemRemovedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
