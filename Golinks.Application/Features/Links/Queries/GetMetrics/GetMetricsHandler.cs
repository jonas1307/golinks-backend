using Golinks.Application.Common;
using Golinks.Application.Responses;
using Golinks.Domain.DTOs;
using Golinks.Repository;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Application.Features.Links.Queries.GetMetrics;

public class GetMetricsHandler(GolinksContext context) : IRequestHandler<GetMetricsQuery, Result<PagedResult<LinkMetricResponse>>>
{
    public async Task<Result<PagedResult<LinkMetricResponse>>> Handle(GetMetricsQuery request, CancellationToken cancellationToken)
    {
        var query = context.Links.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var pattern = $"%{request.Search}%";
            query = query.Where(x =>
                EF.Functions.ILike(x.Url, pattern) ||
                EF.Functions.ILike(x.Slug, pattern) ||
                (x.Description != null && EF.Functions.ILike(x.Description, pattern)));
        }

        var orderedQuery = query.OrderByDescending(x => x.TotalUsage);

        var totalItems = await orderedQuery.CountAsync(cancellationToken);
        var links = await orderedQuery
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

        var result = links.Adapt<IEnumerable<LinkMetricResponse>>()
            .ToDictionary(x => x.Id);

        foreach (var metric in metrics)
            result[metric.LinkId].Metrics.Add(metric.Adapt<MetricResponse>());

        return PagedResult<LinkMetricResponse>.Create(
            result.Values,
            request.PageNumber,
            request.PageSize,
            totalItems,
            request.BaseUrl,
            new Dictionary<string, string?>
            {
                ["metricRange"] = request.MetricRange.ToString(),
                ["search"] = request.Search
            });
    }
}
