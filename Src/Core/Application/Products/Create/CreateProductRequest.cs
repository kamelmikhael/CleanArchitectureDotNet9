namespace Application.Products.Create;

public sealed record CreateProductRequest(
        string Name,
        string Currency,
        decimal Amount,
        string Sku);
