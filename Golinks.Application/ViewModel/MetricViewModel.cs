namespace Golinks.Application.ViewModel
{
    public class MetricViewModel
    {
        public string? Id { get; set; } = null;
        public string LinkId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
