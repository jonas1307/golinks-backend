using Golinks.Application.ViewModel;
using MediatR;

namespace Golinks.Application.Features.Links.Commands.DeleteLink;

public record DeleteLinkCommand(Guid Id) : IRequest<RestResponse<object>>;
