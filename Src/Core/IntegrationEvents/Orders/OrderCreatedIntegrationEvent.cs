namespace IntegrationEvents.Orders;

/// <summary>
/// Order Created Integration Event
/// </summary>
/// <param name="OrderId"></param>
public sealed record OrderCreatedIntegrationEvent(Guid OrderId);
