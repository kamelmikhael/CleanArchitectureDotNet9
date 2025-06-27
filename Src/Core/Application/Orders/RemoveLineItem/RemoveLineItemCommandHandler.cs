using Application.Abstractions.Messaging;
using Application.Orders.Services;
using Domain.Orders;
using SharedKernal.Abstractions.Data;
using SharedKernal.Primitives;

namespace Application.Orders.RemoveLineItem;

internal sealed class RemoveLineItemCommandHandler(
    IGetOrderByIdService orderService) : ICommandHandler<RemoveLineItemCommand>
{
    public async Task<Result> Handle(RemoveLineItemCommand command, CancellationToken cancellationToken)
    {
        Order? order = await orderService.ExecuteAsync(command.OrderId, cancellationToken);

        if (order is null)
        {
            return Result.Failure(OrderErrors.NotFound(command.OrderId));
        }

        order.RemoveLineItem(command.LineItemId);

        return Result.Success();
    }
}
