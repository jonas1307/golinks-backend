using Golinks.Domain.DTOs;
using Golinks.Domain.Entities;
using Golinks.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Repository.Repositories;

public class MetricRepository(GolinksContext context) : BaseRepository<Metric>(context), IMetricRepository
{
    public async Task<IEnumerable<MetricDTO>> GetByLinks(IEnumerable<Guid> links, DateTime startDate, DateTime endDate)
    {
        return await _context.Metrics
            .Where(w => w.CreatedAt >= startDate && w.CreatedAt <= endDate && links.Contains(w.LinkId))
            .GroupBy(m => new
            {
                m.LinkId,
                CreatedDate = m.CreatedAt.Date
            })
            .Select(g => new MetricDTO
            {
                LinkId = g.Key.LinkId,
                Date = g.Key.CreatedDate,
                TotalClicks = g.Count()
            })
            .ToListAsync();
    }
}
