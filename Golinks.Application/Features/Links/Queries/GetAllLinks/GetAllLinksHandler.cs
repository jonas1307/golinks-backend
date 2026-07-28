using Golinks.Application.Common;
using Golinks.Application.Responses;
using Golinks.Repository;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Application.Features.Links.Queries.GetAllLinks;

public class GetAllLinksHandler(GolinksContext context) : IRequestHandler<GetAllLinksQuery, Result<PagedResult<LinkResponse>>>
{
    public async Task<Result<PagedResult<LinkResponse>>> Handle(GetAllLinksQuery request, CancellationToken cancellationToken)
    {
        var query = context.Links.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var term = $"%{request.Search.Trim()}%";
            query = query.Where(l =>
                EF.Functions.ILike(l.Slug, term) ||
                EF.Functions.ILike(l.Url, term) ||
                (l.Description != null && EF.Functions.ILike(l.Description, term)));
        }

        query = query.OrderByDescending(l => l.CreatedAt);

        var totalItems = await query.CountAsync(cancellationToken);
        var data = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return PagedResult<LinkResponse>.Create(
            data.Adapt<IEnumerable<LinkResponse>>(),
            request.PageNumber,
            request.PageSize,
            totalItems,
            request.BaseUrl);
    }
}
