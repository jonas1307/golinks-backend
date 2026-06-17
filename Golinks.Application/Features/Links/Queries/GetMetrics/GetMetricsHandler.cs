using AutoMapper;
using Golinks.Application.Common;
using Golinks.Application.Responses;
using Golinks.Domain.DTOs;
using Golinks.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Application.Features.Links.Queries.GetMetrics;

public class GetMetricsHandler(GolinksContext context, IMapper mapper) : IRequestHandler<GetMetricsQuery, Result<PagedResult<LinkMetricResponse>>>
{
    public async Task<Result<PagedResult<LinkMetricResponse>>> Handle(GetMetricsQuery request, CancellationToken cancellationToken)
    {
        var query = context.Links.AsNoTracking().OrderByDescending(x => x.TotalUsage);

        var totalItems = await query.CountAsync(cancellationToken);
        var links = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var startDate = DateTime.UtcNow.Date.AddDays(request.MetricRange * -1);
        var endDate = DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);

        var linkIds = links.Select(x => x.Id);

        var metrics = await context.Metrics
            .Where(w => w.CreatedAt >= startDate && w.CreatedAt <= endDate && linkIds.Contains(w.LinkId))
            .GroupBy(m => new { m.LinkId, CreatedDate = m.CreatedAt.Date })
            .Select(g => new MetricDTO
            {
                LinkId = g.Key.LinkId,
                Date = g.Key.CreatedDate,
                TotalClicks = g.Count()
            })
            .ToListAsync(cancellationToken);

        var result = mapper.Map<IEnumerable<LinkMetricResponse>>(links)
            .ToDictionary(x => x.Id);

        foreach (var metric in metrics)
            result[metric.LinkId].Metrics.Add(mapper.Map<MetricResponse>(metric));

        return PagedResult<LinkMetricResponse>.Create(
            result.Values,
            request.PageNumber,
            request.PageSize,
            totalItems,
            request.BaseUrl);
    }
}
