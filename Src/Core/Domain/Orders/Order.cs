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

    public void Add(ProductId productId, Money price, int quantity)
        => _lineItems.Add(new(
                new(Guid.NewGuid()), 
                Id, 
                productId, 
                price, 
                quantity)
            );

    public static Order Create(CustomerId customerId)
    {
        var order = new Order(new(Guid.NewGuid()))
        {
            CustomerId = customerId,
        };

        order.Raise(new OrderCreatedDomainEvent(order.Id));

        return order;
    }

    public void RemoveLineItem(LineItemId lineItemId)
    {
        LineItem? lineItem = _lineItems.FirstOrDefault(li => li.Id == lineItemId);

        if (lineItem is null)
        {
            return;
        }

        _lineItems.Remove(lineItem);

        Raise(new LineItemRemovedDomainEvent(Id, lineItemId));
    }
}
