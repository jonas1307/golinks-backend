﻿using Golinks.Domain.Entities;
using Golinks.Repository.Contracts;

namespace Golinks.Repository.Repositories;

public class MetricRepository(GolinksContext context) : RepositoryBase<Metric>(context), IMetricRepository
{
}
