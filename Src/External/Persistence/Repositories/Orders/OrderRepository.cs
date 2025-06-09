using Domain.Orders;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Persistence.Specifications.Orders;

namespace Persistence.Repositories.Orders;

internal sealed class OrderRepository(ApplicationDbContext context) 
    : Repository<Order, OrderId>(context), IOrderRepository
{
    public Task<Order?> GetByOrderIdWithLineItemByIdAsync(
        OrderId orderId, 
        LineItemId lineItemId, 
        CancellationToken cancellationToken = default)
        => ApplySpecification(new OrderByOrderIdWithLineItem(orderId, lineItemId))
            .FirstOrDefaultAsync(cancellationToken);

    public Task<Order?> GetByOrderIdWithLineItemsAsync(
        OrderId orderId, 
        CancellationToken cancellationToken = default)
        => _dbSet
            .Include(x => x.LineItems)
            .AsNoTracking()
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);
}
