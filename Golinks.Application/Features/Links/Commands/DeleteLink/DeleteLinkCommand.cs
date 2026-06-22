using Golinks.Application.Common;
using MediatR;

namespace Golinks.Application.Features.Links.Commands.DeleteLink;

public record DeleteLinkCommand(Guid Id) : IRequest<Result>;
