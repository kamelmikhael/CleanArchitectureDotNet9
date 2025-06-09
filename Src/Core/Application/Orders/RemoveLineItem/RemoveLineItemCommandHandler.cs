using Application.Abstractions.Messaging;
using Domain.Orders;
using SharedKernal.Abstractions.Data;
using SharedKernal.Primitives;

namespace Application.Orders.RemoveLineItem;

internal sealed class RemoveLineItemCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveLineItemCommand>
{
    public async Task<Result> Handle(RemoveLineItemCommand command, CancellationToken cancellationToken)
    {
        Order? order = await orderRepository
            .GetByOrderIdWithLineItemByIdAsync(
                command.OrderId,
                command.LineItemId,
                cancellationToken);

        if (order is null)
        {
            return Result.Failure(OrderErrors.NotFound(command.OrderId));
        }

        order.RemoveLineItem(command.LineItemId);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(order);
    }
}
