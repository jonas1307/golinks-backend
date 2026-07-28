using Golinks.Application.Common;
using Golinks.Application.Responses;
using MediatR;

namespace Golinks.Application.Features.Logs.Queries.GetAccessLogs;

public record GetAccessLogsQuery(
    int PageNumber,
    int PageSize,
    string? BaseUrl,
    Guid? LinkId,
    DateTime? From,
    DateTime? To,
    string? BotFilter) : IRequest<Result<PagedResult<AccessLogResponse>>>;
