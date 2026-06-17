using Golinks.Application.Common;
using Golinks.Application.ViewModel;
using MediatR;

namespace Golinks.Application.Features.Links.Commands.UpdateLink;

public record UpdateLinkCommand(Guid Id, LinkViewModel Model) : IRequest<Result<LinkViewModel>>;
