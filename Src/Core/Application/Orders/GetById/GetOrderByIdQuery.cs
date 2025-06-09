using Application.Abstractions.Messaging;
using Domain.Orders;

namespace Application.Orders.GetById;

public sealed record GetOrderByIdQuery(OrderId OrderId) : IQuery<GetOrderByIdResponse>;

public sealed record GetOrderByIdResponse(Guid OrderId
    , Guid CustomerId
    , List<LineItemResponse> LineItems);

public sealed record LineItemResponse(Guid LineItemId
    , Guid ProductId
    , decimal Price);
