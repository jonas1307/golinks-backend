using AutoMapper;
using Golinks.Application.ViewModel;
using Golinks.Domain.Entities;
using Golinks.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Application.Features.Links.Commands.CreateLink;

public class CreateLinkHandler(GolinksContext context, IMapper mapper) : IRequestHandler<CreateLinkCommand, RestResponse<LinkViewModel>>
{
    public async Task<RestResponse<LinkViewModel>> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
    {
        var slugExists = await context.Links.AnyAsync(x => x.Slug == request.Model.Slug, cancellationToken);

        if (slugExists)
            return RestResponse<LinkViewModel>.Error($"Slug \"{request.Model.Slug}\" already exists.");

        var link = mapper.Map<Link>(request.Model);

        context.Links.Add(link);
        await context.SaveChangesAsync(cancellationToken);

        return RestResponse<LinkViewModel>.Success(mapper.Map<LinkViewModel>(link));
    }
}
