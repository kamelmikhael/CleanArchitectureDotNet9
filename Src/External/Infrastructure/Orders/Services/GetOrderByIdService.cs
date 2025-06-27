using Application.Orders.Services;
using Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Orders.Services;

internal sealed class GetOrderByIdService(
    ApplicationDbContext dbContext) : IGetOrderByIdService
{
    public Task<Order?> ExecuteAsync(OrderId orderId, CancellationToken cancellationToken = default)
    {
        return dbContext.Set<Order>()
            .Include(x => x.LineItems)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == orderId, cancellationToken);
    }
}
