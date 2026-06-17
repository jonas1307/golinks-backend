using AutoMapper;
using Golinks.Application.ViewModel;
using Golinks.Domain.DTOs;
using Golinks.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Application.Features.Actions.Queries.GetLinksWithMetrics;

public class GetLinksWithMetricsHandler(GolinksContext context, IMapper mapper) : IRequestHandler<GetLinksWithMetricsQuery, RestResponse<IEnumerable<LinkMetricViewModel>>>
{
    public async Task<RestResponse<IEnumerable<LinkMetricViewModel>>> Handle(GetLinksWithMetricsQuery request, CancellationToken cancellationToken)
    {
        var query = context.Links.OrderByDescending(x => x.TotalUsage);

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

        var result = mapper.Map<IEnumerable<LinkMetricViewModel>>(links);

        foreach (var metric in metrics)
        {
            var viewModel = mapper.Map<MetricViewModel>(metric);
            result.First(x => x.Id == metric.LinkId).Metrics.Add(viewModel);
        }

        return RestResponse<IEnumerable<LinkMetricViewModel>>.Success(result, request.BaseUrl, request.PageNumber, request.PageSize, totalItems);
    }
}
