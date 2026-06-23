using Golinks.Application.Common;
using Golinks.Application.Responses;
using MediatR;

namespace Golinks.Application.Features.Links.Commands.TrackAccess;

public record TrackAccessCommand(string Slug, string? UserAgent, string? Referrer, string? IpAddress) : IRequest<Result<LinkResponse>>;
