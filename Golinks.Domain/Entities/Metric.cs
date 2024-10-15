namespace Golinks.Domain.Entities;

public class Metric : BaseEntity
{
    public Guid LinkId { get; set; }

    public Link Link { get; set; }
}
