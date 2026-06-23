namespace Golinks.Application.Common;

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; init; } = [];
    public int PageNumber { get; init; }
    public int PageSize { get; init; }
    public int TotalItems { get; init; }
    public int TotalPages { get; init; }
    public bool HasNextPage { get; init; }
    public bool HasPreviousPage { get; init; }
    public string FirstPage { get; init; } = string.Empty;
    public string LastPage { get; init; } = string.Empty;
    public string? NextPage { get; init; }
    public string? PreviousPage { get; init; }

    public static PagedResult<T> Create(
        IEnumerable<T> items,
        int pageNumber,
        int pageSize,
        int totalItems,
        string? baseUrl,
        IReadOnlyDictionary<string, string?>? filters = null)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        var filterQuery = BuildFilterQuery(filters);

        string PageUrl(int page) => $"{baseUrl}?pageNumber={page}&pageSize={pageSize}{filterQuery}";

        return new PagedResult<T>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            HasNextPage = pageNumber < totalPages,
            HasPreviousPage = pageNumber > 1,
            FirstPage = PageUrl(1),
            LastPage = PageUrl(totalPages),
            NextPage = pageNumber < totalPages ? PageUrl(pageNumber + 1) : null,
            PreviousPage = pageNumber > 1 ? PageUrl(pageNumber - 1) : null,
        };
    }

    private static string BuildFilterQuery(IReadOnlyDictionary<string, string?>? filters)
    {
        if (filters is null)
            return string.Empty;

        var parts = filters
            .Where(f => !string.IsNullOrWhiteSpace(f.Value))
            .Select(f => $"&{Uri.EscapeDataString(f.Key)}={Uri.EscapeDataString(f.Value!)}");

        return string.Concat(parts);
    }
}
