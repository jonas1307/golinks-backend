using AutoMapper;
using Golinks.Application.Common;
using Golinks.Application.ViewModel;
using Golinks.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Application.Features.Links.Commands.UpdateLink;

public class UpdateLinkHandler(GolinksContext context, IMapper mapper) : IRequestHandler<UpdateLinkCommand, Result<LinkViewModel>>
{
    public async Task<Result<LinkViewModel>> Handle(UpdateLinkCommand request, CancellationToken cancellationToken)
    {
        var slugTaken = await context.Links.AnyAsync(x => x.Slug == request.Model.Slug && x.Id != request.Id, cancellationToken);

        if (slugTaken)
            return Error.Conflict("Slug already in use.");

        var link = await context.Links.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (link == null)
            return Error.NotFound($"Link with ID {request.Id} was not found.");

        mapper.Map(request.Model, link);

        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<LinkViewModel>(link);
    }
}
