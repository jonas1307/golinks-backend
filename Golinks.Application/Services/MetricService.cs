using Golinks.Application.Contracts;
using Golinks.Domain.Entities;
using Golinks.Repository.Contracts;

namespace Golinks.Application.Services;

public class MetricService(IBaseRepository<Metric> repository) : BaseService<Metric>(repository), IMetricService
{
}
