using Golinks.Application.Common;
using Golinks.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Application.Features.Links.Commands.DeleteLink;

public class DeleteLinkHandler(GolinksContext context) : IRequestHandler<DeleteLinkCommand, Result>
{
    public async Task<Result> Handle(DeleteLinkCommand request, CancellationToken cancellationToken)
    {
        var link = await context.Links.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (link == null)
            return Error.NotFound($"Link with ID {request.Id} was not found.");

        context.Links.Remove(link);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
