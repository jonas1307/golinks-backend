namespace Golinks.Domain.Entities;

public class Link
{
    public string Id { get; set; }
    public string Url { get; set; }
    public string Alias { get; set; }
    public DateTime CreatedAt { get; set; }
}
