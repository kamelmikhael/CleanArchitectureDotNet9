using SharedKernal.Abstraction;

namespace Domain.Orders;

public sealed record LineItemAddedDomainEvent(
    OrderId OrderId,
    LineItemId LineItemId) : IDomainEvent;
