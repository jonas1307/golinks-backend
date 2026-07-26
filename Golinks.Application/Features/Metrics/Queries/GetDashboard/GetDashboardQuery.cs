using Golinks.Application.Common;
using Golinks.Application.Responses;
using MediatR;

namespace Golinks.Application.Features.Metrics.Queries.GetDashboard;

public record GetDashboardQuery(int Days, Guid? LinkId, bool ExcludeBots) : IRequest<Result<DashboardResponse>>;
