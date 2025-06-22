using SharedKernal.Primitives;

namespace Domain.Products;

public class Product : Entity<ProductId>
{
    private Product()
    { }

    private Product(ProductId id) : base(id)
    { }

    public string Name { get; private set; } = string.Empty;

    public Money Price { get; private set; }

    public Sku Sku { get; private set; }

    public static Product Create(string name, Money price, Sku sku)
        => new(new(Guid.NewGuid()))
        {
            Name = name,
            Price = price,
            Sku = sku,
        };

    public void SetName(string name)
    {
        if (Name.Equals(name, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        Name = name;
    }
}
