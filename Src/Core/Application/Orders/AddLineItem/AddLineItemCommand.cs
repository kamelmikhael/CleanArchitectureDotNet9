using Application.Abstractions.Messaging;
using Domain.Orders;
using Domain.Products;

namespace Application.Orders.AddLineItem;

public sealed record AddLineItemCommand(
    OrderId OrderId,
    ProductId ProductId,
    int Qty) : ICommand;
