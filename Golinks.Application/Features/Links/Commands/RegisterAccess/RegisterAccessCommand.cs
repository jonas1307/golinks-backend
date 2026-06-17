using Golinks.Application.Common;
using Golinks.Application.Responses;
using MediatR;

namespace Golinks.Application.Features.Links.Commands.RegisterAccess;

public record RegisterAccessCommand(string Slug) : IRequest<Result<LinkResponse>>;
