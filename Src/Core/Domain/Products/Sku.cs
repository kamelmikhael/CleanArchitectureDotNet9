namespace Domain.Products;

/// <summary>
/// Stock Keeping Unit
/// </summary>
public record Sku
{
    public const int DefaultLength = 8;

    private Sku(string value) => Value = value;

    public string Value { get; init; }

    public static Sku? Create(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length != DefaultLength)
        {
            return null;
        }

        return new(value);
    }
}
