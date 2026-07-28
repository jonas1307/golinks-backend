namespace Golinks.Application.Responses;

public class AccessLogResponse
{
    public DateTime CreatedAt { get; set; }
    public Guid LinkId { get; set; }
    public required string Slug { get; set; }
    public required string Url { get; set; }
    public string? Browser { get; set; }
    public string? Os { get; set; }
    public string? DeviceType { get; set; }
    public string? DeviceModel { get; set; }
    public string? Referrer { get; set; }
    public bool IsBot { get; set; }
}
