using Domain.Orders;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Persistence.Specifications.Orders;

namespace Persistence.Repositories.Orders;

internal sealed class OrderRepository(ApplicationDbContext context) 
    : Repository<Order, OrderId>(context), IOrderRepository
{
    public Task<Order?> GetByOrderIdWithLineItemAsync(
        OrderId orderId, 
        LineItemId lineItemId, 
        CancellationToken cancellationToken = default)
    {
        return ApplySpecification(new OrderByOrderIdWithLineItem(orderId, lineItemId))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
