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

    private int MaxMetricRange { get; } = 120;

    private int _metricRange = 30;

    public int MetricRange
    {
        get
        {
            return _metricRange;
        }
        set
        {
            _metricRange = (value > MaxMetricRange) ? MaxMetricRange : value;
        }
    }
}
