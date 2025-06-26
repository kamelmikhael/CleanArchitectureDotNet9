using SharedKernal.Primitives;

namespace Application.Products.GetPagedList;

public sealed class ProductsPagedRequest : PagedSortedRequest
{
    public string? SearchTerm { get; set; }
}
