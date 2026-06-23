namespace Golinks.Domain.Entities;

public class Link : BaseEntity
{
    public required string Url { get; set; }
    public required string Slug { get; set; }
    public string? Description { get; set; }
    public int TotalUsage { get; set; }
    public IEnumerable<Metric>? Metrics { get; set; }

    public void RegisterUsage()
    {
        TotalUsage += 1;
    }
}
