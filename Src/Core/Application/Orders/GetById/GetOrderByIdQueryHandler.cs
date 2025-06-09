using Application.Abstractions.Messaging;
using Domain.Orders;
using SharedKernal.Primitives;

namespace Application.Orders.GetById;

internal sealed class GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    : IQueryHandler<GetOrderByIdQuery, GetOrderByIdResponse>
{
    public async Task<Result<GetOrderByIdResponse>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        Order? order = await orderRepository
            .GetByOrderIdWithLineItemsAsync(
                query.OrderId,
                cancellationToken);

        if (order is null)
        {
            return Result.Failure<GetOrderByIdResponse>(OrderErrors.NotFound(query.OrderId));
        }

        return new GetOrderByIdResponse(order.Id.Value
            , order.CustomerId.Value
            , [.. order.LineItems
                   .Select(li => new LineItemResponse(li.Id.Value
                                                     , li.ProductId.Value
                                                     , li.Price.Amount))]);
    }
}
