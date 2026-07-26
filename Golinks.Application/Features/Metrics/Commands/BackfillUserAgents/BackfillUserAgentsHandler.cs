using Golinks.Application.Common;
using Golinks.Application.Services;
using Golinks.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Application.Features.Metrics.Commands.BackfillUserAgents;

public class BackfillUserAgentsHandler(GolinksContext context, DashboardCacheService cacheService)
    : IRequestHandler<BackfillUserAgentsCommand, Result<BackfillResult>>
{
    public async Task<Result<BackfillResult>> Handle(BackfillUserAgentsCommand request, CancellationToken cancellationToken)
    {
        var metrics = await context.Metrics
            .Where(m => m.DeviceType == null && m.UserAgent != null)
            .ToListAsync(cancellationToken);

        var updated = 0;
        var skipped = 0;

        foreach (var metric in metrics)
        {
            var parsed = UserAgentParser.Parse(metric.UserAgent);
            if (parsed is null)
            {
                skipped++;
                continue;
            }

            metric.DeviceType = parsed.DeviceType;
            metric.DeviceModel = parsed.DeviceModel;
            metric.Browser = parsed.Browser;
            metric.Os = parsed.Os;
            metric.IsBot = parsed.IsBot;
            updated++;
        }

        await context.SaveChangesAsync(cancellationToken);

        cacheService.Invalidate();

        return new BackfillResult(updated, skipped);
    }
}
