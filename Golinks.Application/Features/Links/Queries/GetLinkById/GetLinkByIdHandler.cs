using AutoMapper;
using Golinks.Application.ViewModel;
using Golinks.Repository;
using MediatR;

namespace Golinks.Application.Features.Links.Queries.GetLinkById;

public class GetLinkByIdHandler(GolinksContext context, IMapper mapper) : IRequestHandler<GetLinkByIdQuery, RestResponse<LinkViewModel>>
{
    public async Task<RestResponse<LinkViewModel>> Handle(GetLinkByIdQuery request, CancellationToken cancellationToken)
    {
        var link = await context.Links.FindAsync([request.Id], cancellationToken);

        if (link == null)
            return RestResponse<LinkViewModel>.Error($"Link with ID {request.Id} was not found.");

        return RestResponse<LinkViewModel>.Success(mapper.Map<LinkViewModel>(link));
    }
}
