using Golinks.Application.Common;
using Golinks.Application.Responses;
using Golinks.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Application.Features.Logs.Queries.GetAccessLogs;

public class GetAccessLogsHandler(GolinksContext context) : IRequestHandler<GetAccessLogsQuery, Result<PagedResult<AccessLogResponse>>>
{
    public async Task<Result<PagedResult<AccessLogResponse>>> Handle(GetAccessLogsQuery request, CancellationToken cancellationToken)
    {
        var query = context.Metrics
            .AsNoTracking()
            .Include(m => m.Link)
            .Where(m => m.Link != null);

        if (request.LinkId.HasValue)
            query = query.Where(m => m.LinkId == request.LinkId.Value);

        if (request.From.HasValue)
        {
            var from = DateTime.SpecifyKind(request.From.Value, DateTimeKind.Utc);
            query = query.Where(m => m.CreatedAt >= from);
        }

        if (request.To.HasValue)
        {
            var to = DateTime.SpecifyKind(request.To.Value, DateTimeKind.Utc);
            query = query.Where(m => m.CreatedAt <= to);
        }

        if (request.BotFilter == "human")
            query = query.Where(m => !m.IsBot);
        else if (request.BotFilter == "bots")
            query = query.Where(m => m.IsBot);

        query = query.OrderByDescending(m => m.CreatedAt);

        var totalItems = await query.CountAsync(cancellationToken);
        var data = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(m => new AccessLogResponse
            {
                CreatedAt = m.CreatedAt,
                LinkId = m.LinkId,
                Slug = m.Link!.Slug,
                Url = m.Link!.Url,
                Browser = m.Browser,
                Os = m.Os,
                DeviceType = m.DeviceType,
                DeviceModel = m.DeviceModel,
                Referrer = m.Referrer,
                IsBot = m.IsBot
            })
            .ToListAsync(cancellationToken);

        return PagedResult<AccessLogResponse>.Create(
            data,
            request.PageNumber,
            request.PageSize,
            totalItems,
            request.BaseUrl);
    }
}
