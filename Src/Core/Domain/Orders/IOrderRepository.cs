using SharedKernal.Abstractions.Data;

namespace Domain.Orders;

public interface IOrderRepository : IRepository<Order, OrderId>
{
    Task<Order?> GetByOrderIdWithLineItemByIdAsync(
        OrderId orderId,
        LineItemId lineItemId,
        CancellationToken cancellationToken = default);

    Task<Order?> GetByOrderIdWithLineItemsAsync(
        OrderId orderId,
        CancellationToken cancellationToken = default);
}
