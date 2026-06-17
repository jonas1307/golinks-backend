using AutoMapper;
using Golinks.Application.ViewModel;
using Golinks.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Application.Features.Links.Queries.GetAllLinks;

public class GetAllLinksHandler(GolinksContext context, IMapper mapper) : IRequestHandler<GetAllLinksQuery, RestResponse<IEnumerable<LinkViewModel>>>
{
    public async Task<RestResponse<IEnumerable<LinkViewModel>>> Handle(GetAllLinksQuery request, CancellationToken cancellationToken)
    {
        var query = context.Links.AsQueryable();

        var totalItems = await query.CountAsync(cancellationToken);
        var data = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return RestResponse<IEnumerable<LinkViewModel>>.Success(
            mapper.Map<IEnumerable<LinkViewModel>>(data),
            request.BaseUrl,
            request.PageNumber,
            request.PageSize,
            totalItems);
    }
}
