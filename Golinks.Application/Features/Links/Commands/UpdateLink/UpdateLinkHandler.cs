using AutoMapper;
using Golinks.Application.ViewModel;
using Golinks.Repository.Contracts;
using MediatR;

namespace Golinks.Application.Features.Links.Commands.UpdateLink;

public class UpdateLinkHandler(ILinkRepository linkRepository, IMapper mapper) : IRequestHandler<UpdateLinkCommand, RestResponse<LinkViewModel>>
{
    public async Task<RestResponse<LinkViewModel>> Handle(UpdateLinkCommand request, CancellationToken cancellationToken)
    {
        var linkWithSameSlug = await linkRepository.FindOneAsync(x => x.Slug == request.Model.Slug);

        if (linkWithSameSlug != null && linkWithSameSlug.Id != request.Id)
            return RestResponse<LinkViewModel>.Error("Slug already in use.");

        var linkInDb = await linkRepository.FindByIdAsync(request.Id);

        if (linkInDb == null)
            return RestResponse<LinkViewModel>.Error($"Link with ID {request.Id} was not found.");

        var link = mapper.Map(request.Model, linkInDb);

        await linkRepository.UpdateAsync(link);

        return RestResponse<LinkViewModel>.Success(mapper.Map<LinkViewModel>(link));
    }
}
