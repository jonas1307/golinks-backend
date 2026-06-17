namespace Golinks.Application.Requests;

public class LinkParams
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
            _pageSize = value < 1 ? 1 : (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
