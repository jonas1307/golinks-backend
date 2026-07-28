using Golinks.Application.Common;
using Golinks.Application.Responses;
using MediatR;

namespace Golinks.Application.Features.Links.Queries.GetAllLinks;

public record GetAllLinksQuery(int PageNumber, int PageSize, string? BaseUrl, string? Search) : IRequest<Result<PagedResult<LinkResponse>>>;
