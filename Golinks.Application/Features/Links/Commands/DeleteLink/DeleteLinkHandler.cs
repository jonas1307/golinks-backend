using Golinks.Application.ViewModel;
using Golinks.Repository;
using MediatR;

namespace Golinks.Application.Features.Links.Commands.DeleteLink;

public class DeleteLinkHandler(GolinksContext context) : IRequestHandler<DeleteLinkCommand, RestResponse<object>>
{
    public async Task<RestResponse<object>> Handle(DeleteLinkCommand request, CancellationToken cancellationToken)
    {
        var link = await context.Links.FindAsync([request.Id], cancellationToken);

        if (link == null)
            return RestResponse<object>.Error($"Link with ID {request.Id} was not found.");

        context.Links.Remove(link);
        await context.SaveChangesAsync(cancellationToken);

        return RestResponse<object>.Success(new { });
    }
}
