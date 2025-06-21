using SharedKernal.Abstraction;

namespace Domain.Orders;

/// <summary>
/// Represents a domain event that is triggered when an order is created.
/// </summary>
/// <remarks>This event is typically used to notify other parts of the system about the creation of a new order.
/// It contains the unique identifier of the created order.</remarks>
/// <param name="OrderId">The unique identifier of the created order.</param>
public sealed record OrderCreatedDomainEvent(OrderId OrderId) : IDomainEvent;
