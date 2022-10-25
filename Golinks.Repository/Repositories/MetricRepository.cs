using Golinks.Domain.Entities;
using Golinks.Repository.Contracts;
using Golinks.Repository.Extensions.Settings;

namespace Golinks.Repository.Repositories;

public class MetricRepository : RepositoryBase<Metric>, IMetricRepository
{
    public MetricRepository(IMongoDbSettings settings) : base(settings)
    { }
}
