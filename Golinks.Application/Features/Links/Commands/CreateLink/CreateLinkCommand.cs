using Golinks.Application.Common;
using Golinks.Application.Requests;
using Golinks.Application.Responses;
using MediatR;

namespace Golinks.Application.Features.Links.Commands.CreateLink;

public record CreateLinkCommand(LinkRequest Model) : IRequest<Result<LinkResponse>>;
