using Golinks.Application.Common;
using Golinks.Application.Responses;
using MediatR;

namespace Golinks.Application.Features.Links.Commands.TrackAccess;

public record TrackAccessCommand(string Slug) : IRequest<Result<LinkResponse>>;
