using Golinks.Application.Common;
using Golinks.Application.Responses;
using Golinks.Repository;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Application.Features.Links.Commands.UpdateLink;

public class UpdateLinkHandler(GolinksContext context) : IRequestHandler<UpdateLinkCommand, Result<LinkResponse>>
{
    public async Task<Result<LinkResponse>> Handle(UpdateLinkCommand request, CancellationToken cancellationToken)
    {
        var slugTaken = await context.Links.AnyAsync(x => x.Slug == request.Model.Slug && x.Id != request.Id, cancellationToken);

        if (slugTaken)
            return Error.Conflict("Slug already in use.");

        var link = await context.Links.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (link == null)
            return Error.NotFound($"Link with ID {request.Id} was not found.");

        request.Model.Adapt(link);

        await context.SaveChangesAsync(cancellationToken);

        return link.Adapt<LinkResponse>();
    }
}
