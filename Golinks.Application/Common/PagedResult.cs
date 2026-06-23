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

    public static PagedResult<T> Create(IEnumerable<T> items, int pageNumber, int pageSize, int totalItems, string baseUrl)
    {
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        return new PagedResult<T>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            HasNextPage = pageNumber < totalPages,
            HasPreviousPage = pageNumber > 1,
            FirstPage = $"{baseUrl}?pageNumber=1&pageSize={pageSize}",
            LastPage = $"{baseUrl}?pageNumber={totalPages}&pageSize={pageSize}",
            NextPage = pageNumber < totalPages ? $"{baseUrl}?pageNumber={pageNumber + 1}&pageSize={pageSize}" : null,
            PreviousPage = pageNumber > 1 ? $"{baseUrl}?pageNumber={pageNumber - 1}&pageSize={pageSize}" : null,
        };
    }
}
