namespace Golinks.Application.Requests;

public class LinkRequest
{
    public required string Url { get; set; }
    public required string Slug { get; set; }
    public string? Description { get; set; }
}
