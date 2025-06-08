using Domain.Products;
using Domain.Users;
using SharedKernal.Primitives;

namespace Domain.Orders;

public class Order
{
    private Order()
    { }

    public OrderId Id { get; private set; }

    public UserId UserId { get; private set; }

    private readonly HashSet<LineItem> _lineItems = new();

    public IReadOnlySet<LineItem> LineItems => _lineItems;

    public void Add(ProductId productId, Money price, int quantity)
        => _lineItems.Add(new(
                new(Guid.NewGuid()), 
                Id, 
                productId, 
                price, 
                quantity)
            );

    public static Order Create(UserId userId) => new() 
        {
            Id = new(Guid.NewGuid()),
            UserId = userId 
        };
}
