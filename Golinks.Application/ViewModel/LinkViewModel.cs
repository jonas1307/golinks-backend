using System.ComponentModel.DataAnnotations;

namespace Golinks.Application.ViewModel;

public class LinkViewModel
{
    public Guid? Id { get; set; }

    [Required]
    [Url]
    [MaxLength(2048)]
    public string Url { get; set; }

    [Required]
    [MaxLength(100)]
    public string Slug { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    public DateTime? CreatedAt { get; set; }
    public int TotalUsage { get; set; }
}
