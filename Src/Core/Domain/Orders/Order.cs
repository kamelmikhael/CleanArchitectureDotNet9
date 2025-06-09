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

    public static Order Create(CustomerId customerId) => new(new(Guid.NewGuid())) 
        {
            CustomerId = customerId, 
        };

    public void RemoveLineItem(LineItemId lineItemId)
    {
        _lineItems.RemoveWhere(li => li.Id == lineItemId);
    }
}
