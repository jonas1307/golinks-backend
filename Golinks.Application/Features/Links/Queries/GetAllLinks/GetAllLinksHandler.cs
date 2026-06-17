using AutoMapper;
using Golinks.Application.ViewModel;
using Golinks.Repository.Contracts;
using MediatR;

namespace Golinks.Application.Features.Links.Queries.GetAllLinks;

public class GetAllLinksHandler(ILinkRepository linkRepository, IMapper mapper) : IRequestHandler<GetAllLinksQuery, RestResponse<IEnumerable<LinkViewModel>>>
{
    public async Task<RestResponse<IEnumerable<LinkViewModel>>> Handle(GetAllLinksQuery request, CancellationToken cancellationToken)
    {
        var (data, totalItems) = await linkRepository.FindAllAsync(request.PageNumber, request.PageSize);

        return RestResponse<IEnumerable<LinkViewModel>>.Success(
            mapper.Map<IEnumerable<LinkViewModel>>(data),
            request.BaseUrl,
            request.PageNumber,
            request.PageSize,
            totalItems);
    }
}
