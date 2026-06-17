using Golinks.Application.Common;
using Golinks.Application.ViewModel;
using MediatR;

namespace Golinks.Application.Features.Links.Commands.CreateLink;

public record CreateLinkCommand(LinkViewModel Model) : IRequest<Result<LinkViewModel>>;
