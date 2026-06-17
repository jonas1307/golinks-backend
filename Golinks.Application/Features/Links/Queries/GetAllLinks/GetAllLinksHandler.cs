using AutoMapper;
using Golinks.Application.Common;
using Golinks.Application.ViewModel;
using Golinks.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Application.Features.Links.Queries.GetAllLinks;

public class GetAllLinksHandler(GolinksContext context, IMapper mapper) : IRequestHandler<GetAllLinksQuery, Result<PagedResult<LinkViewModel>>>
{
    public async Task<Result<PagedResult<LinkViewModel>>> Handle(GetAllLinksQuery request, CancellationToken cancellationToken)
    {
        var query = context.Links.AsNoTracking();

        var totalItems = await query.CountAsync(cancellationToken);
        var data = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return PagedResult<LinkViewModel>.Create(
            mapper.Map<IEnumerable<LinkViewModel>>(data),
            request.PageNumber,
            request.PageSize,
            totalItems,
            request.BaseUrl);
    }
}
