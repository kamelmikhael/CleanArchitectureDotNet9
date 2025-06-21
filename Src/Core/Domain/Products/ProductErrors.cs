using SharedKernal.Primitives;

namespace Domain.Products;

public static class ProductErrors
{
    public static Error NotFound(ProductId productId) => Error.NotFound(
        "Products.NotFound",
        $"The product with the Id = '{productId.Value}' was not found");
}
