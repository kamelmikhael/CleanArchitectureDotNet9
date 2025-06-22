using Domain.Orders;
using Microsoft.Extensions.Logging;
using SharedKernal.Abstraction;

namespace Application.Orders.RemoveLineItem;

internal sealed class LineItemRemovedDomainEventHandler(
    ILogger<LineItemRemovedDomainEventHandler> logger) : IDomainEventHandler<LineItemRemovedDomainEvent>
{
    public Task HandleAsync(
        LineItemRemovedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Line Item Removed For OrderId = {OrderId}, and LineItemId = {LineItemId} at {Time} UTC"
            , domainEvent.OrderId
            , domainEvent.LineItemId
            , DateTime.UtcNow);

        return Task.CompletedTask;
    }
}
