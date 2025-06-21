namespace Application.Orders.Create;

/// <summary>
/// Order Created Integration Event
/// </summary>
/// <param name="OrderId"></param>
public sealed record OrderCreatedEvent(Guid OrderId);
