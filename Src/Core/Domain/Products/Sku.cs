namespace Domain.Products;

/// <summary>
/// Stock Keeping Unit
/// </summary>
public sealed record Sku
{
    public const int DefaultLength = 8;

    private Sku(string value) => Value = value;

    public string Value { get; init; }

    public static explicit operator string(Sku sku) => sku.Value;

    public static Sku? Create(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length != DefaultLength)
        {
            return null;
        }

        return new(value);
    }
}
