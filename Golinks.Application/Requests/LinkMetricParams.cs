namespace Golinks.Application.Requests;

public class LinkMetricParams
{
    private int MaxPageSize { get; } = 50;

    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10;

    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }

    public DateTime StartDate { get; set; } = DateTime.UtcNow.Date.AddDays(-30);
    
    public DateTime EndDate { get; set; } = DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);
}
