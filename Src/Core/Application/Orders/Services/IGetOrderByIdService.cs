using Domain.Orders;

namespace Application.Orders.Services;

public interface IGetOrderByIdService
{
    Task<Order?> ExecuteAsync(OrderId orderId, CancellationToken cancellationToken = default);
}
