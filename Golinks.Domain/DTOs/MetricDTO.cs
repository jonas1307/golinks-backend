
namespace Golinks.Domain.DTOs
{
    public class MetricDTO
    {
        public Guid LinkId { get; set; }
        public DateTime Date { get; set; }
        public int TotalClicks { get; set; }
    }
}
