using Application.Abstractions.Messaging;
using Domain.Orders;

namespace Application.Orders.RemoveLineItem;

public sealed record RemoveLineItemCommand(OrderId OrderId, LineItemId LineItemId)
    : ICommand;
