namespace Golinks.Application.Requests;

public class LogsParams
{
    private int MaxPageSize { get; } = 50;

    public int PageNumber { get; set; } = 1;

    private int _pageSize = 50;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 1 : (value > MaxPageSize) ? MaxPageSize : value;
    }

    public Guid? LinkId { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public string? BotFilter { get; set; } // "all" | "human" | "bots"
}
