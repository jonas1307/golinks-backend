using Golinks.Application.Common;
using Golinks.Application.ViewModel;
using MediatR;

namespace Golinks.Application.Features.Links.Commands.RegisterAccess;

public record RegisterAccessCommand(string Slug) : IRequest<Result<LinkViewModel>>;
