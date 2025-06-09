using Domain.Orders;

namespace Persistence.Specifications.Orders;

internal sealed class OrderByOrderIdWithLineItem
    : Specification<Order, OrderId>
{
    public OrderByOrderIdWithLineItem(
        OrderId orderId, LineItemId lineItemId) : base(x => x.Id == orderId)
    {
        Include(x => x.LineItems.Where(li => li.Id == lineItemId));
    }
}
