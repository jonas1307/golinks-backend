namespace Golinks.Application.Responses;

public class LinkMetricResponse
{
    public Guid Id { get; set; }
    public required string Url { get; set; }
    public required string Slug { get; set; }
    public string? Description { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int? MaxUsage { get; set; }
    public DateTime CreatedAt { get; set; }
    public int TotalUsage { get; set; }
    public List<MetricResponse> Metrics { get; set; } = [];
}
