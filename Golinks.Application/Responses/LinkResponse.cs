namespace Golinks.Application.Responses;

public class LinkResponse
{
    public Guid Id { get; set; }
    public required string Url { get; set; }
    public required string Slug { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int TotalUsage { get; set; }
}
