using Domain.Orders;
using SharedKernal.Abstraction;

namespace Application.Orders.Create;

public sealed record OrderCreatedEvent(Guid OrderId);


