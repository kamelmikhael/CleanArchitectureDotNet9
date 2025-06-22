using SharedKernal.Abstraction;

namespace Domain.Orders;

/// <summary>
/// Represents a domain event that occurs when a line item is removed from an order.
/// </summary>
/// <remarks>This event is typically used to notify other parts of the system about the removal of a line item
/// from an order, enabling actions such as updating order summaries or triggering workflows.</remarks>
/// <param name="OrderId"></param>
/// <param name="LineItemId"></param>
public sealed record LineItemRemovedDomainEvent(
    OrderId OrderId, 
    LineItemId LineItemId) : IDomainEvent;
