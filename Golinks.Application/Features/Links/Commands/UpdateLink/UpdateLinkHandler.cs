using AutoMapper;
using Golinks.Application.ViewModel;
using Golinks.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Application.Features.Links.Commands.UpdateLink;

public class UpdateLinkHandler(GolinksContext context, IMapper mapper) : IRequestHandler<UpdateLinkCommand, RestResponse<LinkViewModel>>
{
    public async Task<RestResponse<LinkViewModel>> Handle(UpdateLinkCommand request, CancellationToken cancellationToken)
    {
        var linkWithSameSlug = await context.Links.FirstOrDefaultAsync(x => x.Slug == request.Model.Slug, cancellationToken);

        if (linkWithSameSlug != null && linkWithSameSlug.Id != request.Id)
            return RestResponse<LinkViewModel>.Error("Slug already in use.");

        var linkInDb = await context.Links.FindAsync([request.Id], cancellationToken);

        if (linkInDb == null)
            return RestResponse<LinkViewModel>.Error($"Link with ID {request.Id} was not found.");

        mapper.Map(request.Model, linkInDb);

        await context.SaveChangesAsync(cancellationToken);

        return RestResponse<LinkViewModel>.Success(mapper.Map<LinkViewModel>(linkInDb));
    }
}
