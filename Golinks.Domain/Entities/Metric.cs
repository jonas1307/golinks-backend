namespace Golinks.Domain.Entities;

public class Metric : BaseEntity
{
    public Guid LinkId { get; set; }
    public string? UserAgent { get; set; }
    public string? Referrer { get; set; }
    public string? IpHash { get; set; }
    public string? DeviceType { get; set; }
    public string? DeviceModel { get; set; }
    public string? Browser { get; set; }
    public string? Os { get; set; }
    public bool IsBot { get; set; }

    public Link? Link { get; set; }
}
