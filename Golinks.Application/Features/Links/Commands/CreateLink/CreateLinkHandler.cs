using AutoMapper;
using Golinks.Application.Common;
using Golinks.Application.Responses;
using Golinks.Domain.Entities;
using Golinks.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Application.Features.Links.Commands.CreateLink;

public class CreateLinkHandler(GolinksContext context, IMapper mapper) : IRequestHandler<CreateLinkCommand, Result<LinkResponse>>
{
    public async Task<Result<LinkResponse>> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
    {
        var slugExists = await context.Links.AnyAsync(x => x.Slug == request.Model.Slug, cancellationToken);

        if (slugExists)
            return Error.Conflict($"Slug \"{request.Model.Slug}\" already exists.");

        var link = mapper.Map<Link>(request.Model);

        context.Links.Add(link);
        await context.SaveChangesAsync(cancellationToken);

        return mapper.Map<LinkResponse>(link);
    }
}
