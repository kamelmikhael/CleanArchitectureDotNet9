namespace Application.Orders.AddLineItem;

public sealed record AddLineItemRequest(
    Guid ProductId,
    int Qty);
