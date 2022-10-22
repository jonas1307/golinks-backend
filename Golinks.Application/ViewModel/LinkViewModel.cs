namespace Golinks.Application.ViewModel;

public class LinkViewModel
{
    public string? Id { get; set; } = null;
    public string Url { get; set; }
    public string Alias { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
