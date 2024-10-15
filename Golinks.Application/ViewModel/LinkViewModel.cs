namespace Golinks.Application.ViewModel;

public class LinkViewModel
{
    public Guid? Id { get; set; }
    public string Url { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public DateTime? CreatedAt { get; set; }
}
