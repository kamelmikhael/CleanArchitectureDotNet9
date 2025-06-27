using Domain.Customers;
using Domain.Products;
using SharedKernal.Primitives;

namespace Domain.Orders;

public class Order : Entity<OrderId>
{
    private Order() : base()
    { }

    private Order(OrderId id) : base(id)
    { }

    public CustomerId CustomerId { get; private set; }

    private readonly HashSet<LineItem> _lineItems = [];

    public IReadOnlyList<LineItem> LineItems => [.. _lineItems];

    public Result<LineItem> AddLineItem(ProductId productId, Money price, int quantity)
    {
        var lineItem = new LineItem(
                new(Guid.NewGuid()),
                Id,
                productId,
                price,
                quantity);

        _lineItems.Add(lineItem);

        Raise(new LineItemAddedDomainEvent(Id, lineItem.Id));

        return lineItem;
    }

    public void RemoveLineItem(LineItemId lineItemId)
    {
        if (HasOneLineItem())
        {
            return;
        }

        LineItem? lineItem = _lineItems.FirstOrDefault(li => li.Id == lineItemId);

        if (lineItem is null)
        {
            return;
        }

        _lineItems.Remove(lineItem);

        Raise(new LineItemRemovedDomainEvent(Id, lineItemId));
    }

    public bool HasOneLineItem() => _lineItems.Count == 1;

    public static Order Create(CustomerId customerId)
    {
        var order = new Order(new(Guid.NewGuid()))
        {
            CustomerId = customerId,
        };

        order.Raise(new OrderCreatedDomainEvent(order.Id));

        return order;
    }
}
