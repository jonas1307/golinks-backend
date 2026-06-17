using Golinks.Application.Common;
using Golinks.Application.ViewModel;
using MediatR;

namespace Golinks.Application.Features.Actions.Queries.GetLinksWithMetrics;

public record GetLinksWithMetricsQuery(int PageNumber, int PageSize, int MetricRange, string BaseUrl) : IRequest<Result<PagedResult<LinkMetricViewModel>>>;
