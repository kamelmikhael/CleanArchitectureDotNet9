namespace SharedKernal.Primitives;

public class PagedResult<TValue> : Result
{
    protected internal PagedResult(
        List<TValue> data,
        int totalCount,
        int pageIndex,
        int pageSize,
        bool isSuccess,
        Error error) : base(isSuccess, error)
    {
        Data = data;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    protected internal PagedResult(
        List<TValue> data,
        int totalCount,
        int pageIndex,
        int pageSize, 
        bool isSuccess, 
        Error[] errors) : base(isSuccess, errors)
    {
        Data = data;
        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }

    /// <summary>
    /// The data result.
    /// </summary>
    public List<TValue> Data { get; private set; }

    /// <summary>
    /// Zero-based index of current page.
    /// </summary>
    public int PageIndex { get; private set; }

    /// <summary>
    /// Number of items contained in each page.
    /// </summary>
    public int PageSize { get; private set; }

    /// <summary>
    /// Total items count
    /// </summary>
    public int TotalCount { get; private set; }

    /// <summary>
    /// Total pages count
    /// </summary>
    public int TotalPages { get; private set; }

    /// <summary>
    /// TRUE if the current page has a previous page,
    /// FALSE otherwise.
    /// </summary>
    public bool HasPreviousPage => PageIndex > 0;

    /// <summary>
    /// TRUE if the current page has a next page, FALSE otherwise.
    /// </summary>
    public bool HasNextPage => PageIndex + 1 < TotalPages;

    public static PagedResult<TValue> Create(
        List<TValue> data,
        int totalCount,
        int pageIndex,
        int pageSize)
        => new(
            data,
            totalCount,
            pageIndex,
            pageSize,
            true,
            Error.None);
}
