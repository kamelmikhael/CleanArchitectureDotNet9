using SharedKernal.Primitives;

namespace Domain.Products;

public class Product
{
    private Product()
    { }

    public ProductId Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public Money Price { get; private set; }

    public Sku Sku { get; private set; }

    public static Product Create(string name, Money price, Sku sku)
        => new()
        {
            Id = new(Guid.NewGuid()),
            Name = name,
            Price = price,
            Sku = sku,
        };
}
