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
        var slugTaken = await context.Links.AnyAsync(x => x.Slug == request.Model.Slug && x.Id != request.Id, cancellationToken);

        if (slugTaken)
            return RestResponse<LinkViewModel>.Error("Slug already in use.");

        var link = await context.Links.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (link == null)
            return RestResponse<LinkViewModel>.Error($"Link with ID {request.Id} was not found.");

        mapper.Map(request.Model, link);

        await context.SaveChangesAsync(cancellationToken);

        return RestResponse<LinkViewModel>.Success(mapper.Map<LinkViewModel>(link));
    }
}
