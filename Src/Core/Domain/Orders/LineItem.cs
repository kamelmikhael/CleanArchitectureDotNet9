using Domain.Products;
using SharedKernal.Primitives;

namespace Domain.Orders;

public class LineItem
{
    internal LineItem(LineItemId id
        , OrderId orderId
        , ProductId productId
        , Money price
        , int quantity)
        => (Id, OrderId, ProductId, Price, Quantity) = (id, orderId, productId, price, quantity);

    public LineItemId Id { get; private set; }

    public OrderId OrderId { get; private set; }

    public ProductId ProductId { get; private set; }

    public Money Price { get; private set; }

    public int Quantity { get; set; }
}
