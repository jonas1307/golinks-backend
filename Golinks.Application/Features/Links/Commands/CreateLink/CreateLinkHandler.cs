using AutoMapper;
using Golinks.Application.ViewModel;
using Golinks.Domain.Entities;
using Golinks.Repository.Contracts;
using MediatR;

namespace Golinks.Application.Features.Links.Commands.CreateLink;

public class CreateLinkHandler(ILinkRepository linkRepository, IMapper mapper) : IRequestHandler<CreateLinkCommand, RestResponse<LinkViewModel>>
{
    public async Task<RestResponse<LinkViewModel>> Handle(CreateLinkCommand request, CancellationToken cancellationToken)
    {
        var existing = await linkRepository.FindOneAsync(x => x.Slug == request.Model.Slug);

        if (existing != null)
            return RestResponse<LinkViewModel>.Error($"Slug \"{request.Model.Slug}\" already exists.");

        var link = mapper.Map<Link>(request.Model);

        await linkRepository.CreateAsync(link);

        return RestResponse<LinkViewModel>.Success(mapper.Map<LinkViewModel>(link));
    }
}
