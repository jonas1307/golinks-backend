namespace Golinks.Domain.Entities;

public class Link : BaseEntity
{
    public required string Url { get; set; }
    public required string Slug { get; set; }
    public string? Description { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public int TotalUsage { get; set; }
    public IEnumerable<Metric>? Metrics { get; set; }

    public bool IsExpired => ExpiresAt.HasValue && ExpiresAt.Value <= DateTime.UtcNow;

    public void RegisterUsage()
    {
        TotalUsage += 1;
    }
}
