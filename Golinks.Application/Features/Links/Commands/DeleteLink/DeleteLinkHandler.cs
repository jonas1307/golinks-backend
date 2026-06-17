using Golinks.Application.ViewModel;
using Golinks.Repository.Contracts;
using MediatR;

namespace Golinks.Application.Features.Links.Commands.DeleteLink;

public class DeleteLinkHandler(ILinkRepository linkRepository) : IRequestHandler<DeleteLinkCommand, RestResponse<object>>
{
    public async Task<RestResponse<object>> Handle(DeleteLinkCommand request, CancellationToken cancellationToken)
    {
        var link = await linkRepository.FindByIdAsync(request.Id);

        if (link == null)
            return RestResponse<object>.Error($"Link with ID {request.Id} was not found.");

        await linkRepository.DeleteAsync(link);

        return RestResponse<object>.Success(new { });
    }
}
