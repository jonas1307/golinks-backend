using Golinks.Application.Common;
using Golinks.Application.Requests;
using Golinks.Application.Responses;
using MediatR;

namespace Golinks.Application.Features.Links.Commands.UpdateLink;

public record UpdateLinkCommand(Guid Id, LinkRequest Model) : IRequest<Result<LinkResponse>>;
