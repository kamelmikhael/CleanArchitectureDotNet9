namespace Application.Products.GetPagedList;

public sealed record ProductsPagedResponse(Guid Id
    , string Name
    , string? Sku
    , decimal? Amount
    , string? Currency);
