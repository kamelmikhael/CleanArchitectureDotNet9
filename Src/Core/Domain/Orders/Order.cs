using Domain.Customers;
using Domain.Products;
using Domain.Users;
using SharedKernal.Primitives;

namespace Domain.Orders;

public class Order
{
    private Order()
    { }

    public OrderId Id { get; private set; }

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

    public static Order Create(CustomerId customerId) => new() 
        {
            Id = new(Guid.NewGuid()),
            CustomerId = customerId, 
        };
}
