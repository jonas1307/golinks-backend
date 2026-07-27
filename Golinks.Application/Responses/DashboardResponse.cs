namespace Golinks.Application.Responses;

public class DashboardResponse
{
    public int TotalClicks { get; set; }
    public int BotClicks { get; set; }
    public int UniqueVisitors { get; set; }
    public int NewVisitors { get; set; }
    public int ReturningVisitors { get; set; }
    public double AvgClicksPerVisitor { get; set; }
    public List<LabelCountResponse> ByDevice { get; set; } = [];
    public List<LabelCountResponse> ByDeviceModel { get; set; } = [];
    public List<LabelCountResponse> ByBrowser { get; set; } = [];
    public List<LabelCountResponse> ByOs { get; set; } = [];
    public List<DailyCountResponse> ClicksOverTime { get; set; } = [];
    public List<HeatmapEntryResponse> Heatmap { get; set; } = [];
}

public class LabelCountResponse
{
    public required string Label { get; set; }
    public int Count { get; set; }
}

public class DailyCountResponse
{
    public required string Date { get; set; }
    public int Count { get; set; }
}

public class HeatmapEntryResponse
{
    public int DayOfWeek { get; set; }
    public int Hour { get; set; }
    public int Count { get; set; }
}
