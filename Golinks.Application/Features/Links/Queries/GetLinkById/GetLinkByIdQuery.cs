using Golinks.Application.Common;
using Golinks.Application.Responses;
using MediatR;

namespace Golinks.Application.Features.Links.Queries.GetLinkById;

public record GetLinkByIdQuery(Guid Id) : IRequest<Result<LinkResponse>>;
