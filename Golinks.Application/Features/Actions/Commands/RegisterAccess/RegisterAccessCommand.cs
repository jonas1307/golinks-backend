using Golinks.Application.Common;
using Golinks.Application.ViewModel;
using MediatR;

namespace Golinks.Application.Features.Actions.Commands.RegisterAccess;

public record RegisterAccessCommand(string Slug) : IRequest<Result<LinkViewModel>>;
