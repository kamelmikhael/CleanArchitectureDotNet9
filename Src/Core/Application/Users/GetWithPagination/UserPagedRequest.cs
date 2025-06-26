using SharedKernal.Primitives;

namespace Application.Users.GetWithPagination;

public sealed class UserPagedRequest : PagedRequest
{
    /// <summary>
    /// Search Keyword
    /// </summary>
    public string? Keyword { get; set; }
}
