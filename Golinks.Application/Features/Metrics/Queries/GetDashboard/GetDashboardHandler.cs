using Golinks.Application.Common;
using Golinks.Application.Responses;
using Golinks.Application.Services;
using Golinks.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Golinks.Application.Features.Metrics.Queries.GetDashboard;

public class GetDashboardHandler(GolinksContext context, IMemoryCache cache, DashboardCacheService cacheService)
    : IRequestHandler<GetDashboardQuery, Result<DashboardResponse>>
{
    public async Task<Result<DashboardResponse>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = cacheService.BuildKey(request.Days, request.LinkId, request.ExcludeBots);

        if (cache.TryGetValue(cacheKey, out DashboardResponse? cached) && cached is not null)
            return cached;

        var startDate = DateTime.UtcNow.Date.AddDays(-request.Days);

        var query = context.Metrics
            .AsNoTracking()
            .Where(m => m.CreatedAt >= startDate);

        if (request.LinkId.HasValue)
            query = query.Where(m => m.LinkId == request.LinkId.Value);

        if (request.ExcludeBots)
            query = query.Where(m => !m.IsBot);

        var metrics = await query
            .Select(m => new
            {
                m.CreatedAt,
                m.DeviceType,
                m.DeviceModel,
                m.Browser,
                m.Os,
                m.IsBot,
                m.IpHash
            })
            .ToListAsync(cancellationToken);

        var totalClicks = metrics.Count;
        var botClicks = metrics.Count(m => m.IsBot);

        var visitorsByIp = metrics
            .Where(m => m.IpHash != null)
            .GroupBy(m => m.IpHash!)
            .ToList();

        var uniqueVisitors = visitorsByIp.Count;
        var returningVisitors = visitorsByIp.Count(g => g.Select(m => m.CreatedAt.Date).Distinct().Count() > 1);
        var newVisitors = uniqueVisitors - returningVisitors;
        var trackedClicks = visitorsByIp.Sum(g => g.Count());
        var avgClicksPerVisitor = uniqueVisitors > 0 ? Math.Round((double)trackedClicks / uniqueVisitors, 1) : 0;

        var byDevice = metrics
            .Where(m => m.DeviceType != null)
            .GroupBy(m => m.DeviceType!)
            .Select(g => new LabelCountResponse { Label = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToList();

        var byDeviceModel = metrics
            .Where(m => m.DeviceModel != null)
            .GroupBy(m => m.DeviceModel!)
            .Select(g => new LabelCountResponse { Label = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(15)
            .ToList();

        var byBrowser = metrics
            .Where(m => m.Browser != null)
            .GroupBy(m => m.Browser!)
            .Select(g => new LabelCountResponse { Label = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToList();

        var byOs = metrics
            .Where(m => m.Os != null)
            .GroupBy(m => m.Os!)
            .Select(g => new LabelCountResponse { Label = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToList();

        var clicksOverTime = metrics
            .GroupBy(m => m.CreatedAt.Date)
            .Select(g => new DailyCountResponse
            {
                Date = g.Key.ToString("yyyy/MM/dd"),
                Count = g.Count()
            })
            .OrderBy(x => x.Date)
            .ToList();

        var heatmap = metrics
            .GroupBy(m => new { DayOfWeek = (int)m.CreatedAt.DayOfWeek, Hour = m.CreatedAt.Hour })
            .Select(g => new HeatmapEntryResponse
            {
                DayOfWeek = g.Key.DayOfWeek,
                Hour = g.Key.Hour,
                Count = g.Count()
            })
            .ToList();

        var response = new DashboardResponse
        {
            TotalClicks = totalClicks,
            BotClicks = botClicks,
            UniqueVisitors = uniqueVisitors,
            NewVisitors = newVisitors,
            ReturningVisitors = returningVisitors,
            AvgClicksPerVisitor = avgClicksPerVisitor,
            ByDevice = byDevice,
            ByDeviceModel = byDeviceModel,
            ByBrowser = byBrowser,
            ByOs = byOs,
            ClicksOverTime = clicksOverTime,
            Heatmap = heatmap
        };

        cache.Set(cacheKey, response, TimeSpan.FromMinutes(5));

        return response;
    }
}
