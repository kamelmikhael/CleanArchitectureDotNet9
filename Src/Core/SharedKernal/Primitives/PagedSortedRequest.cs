namespace SharedKernal.Primitives;

public class PagedSortedRequest : PagedRequest
{
    public string? SortColumn { get; set; }

    public string? SortOrder { get; set; }
}
