using SharedKernal.Primitives;

namespace Application.Users.GetWithPagination;

public class UserPagedRequest : PagedRequest
{
    /// <summary>
    /// Search Keyword
    /// </summary>
    public string? Keyword { get; set; }
}
