using Golinks.Domain.DTOs;
using Golinks.Domain.Entities;

namespace Golinks.Repository.Contracts;

public interface IMetricRepository : IBaseRepository<Metric>
{
    Task<IEnumerable<MetricDTO>> GetByLinks(IEnumerable<Guid> links, DateTime startDate, DateTime endDate);
}
