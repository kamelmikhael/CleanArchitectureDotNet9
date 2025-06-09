using SharedKernal.Abstractions.Data;

namespace Domain.Orders;

public interface IOrderRepository : IRepository<Order, OrderId>
{
    Task<Order?> GetByOrderIdWithLineItemAsync(
        OrderId orderId,
        LineItemId lineItemId,
        CancellationToken cancellationToken = default);
}
