using Golinks.Application.Common;
using Golinks.Application.ViewModel;
using MediatR;

namespace Golinks.Application.Features.Links.Queries.GetAllLinks;

public record GetAllLinksQuery(int PageNumber, int PageSize, string BaseUrl) : IRequest<Result<PagedResult<LinkViewModel>>>;
