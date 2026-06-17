using AutoMapper;
using Golinks.Application.Common;
using Golinks.Application.Responses;
using Golinks.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Application.Features.Links.Queries.GetLinkById;

public class GetLinkByIdHandler(GolinksContext context, IMapper mapper) : IRequestHandler<GetLinkByIdQuery, Result<LinkResponse>>
{
    public async Task<Result<LinkResponse>> Handle(GetLinkByIdQuery request, CancellationToken cancellationToken)
    {
        var link = await context.Links
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (link == null)
            return Error.NotFound($"Link with ID {request.Id} was not found.");

        return mapper.Map<LinkResponse>(link);
    }
}
