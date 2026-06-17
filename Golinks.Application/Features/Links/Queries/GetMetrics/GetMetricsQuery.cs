using Golinks.Application.Common;
using Golinks.Application.Responses;
using MediatR;

namespace Golinks.Application.Features.Links.Queries.GetMetrics;

public record GetMetricsQuery(int PageNumber, int PageSize, int MetricRange, string BaseUrl) : IRequest<Result<PagedResult<LinkMetricResponse>>>;
