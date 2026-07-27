using Golinks.Application.Common;
using MediatR;

namespace Golinks.Application.Features.Metrics.Commands.BackfillUserAgents;

public record BackfillUserAgentsCommand : IRequest<Result<BackfillResult>>;

public record BackfillResult(int Updated, int Skipped);
