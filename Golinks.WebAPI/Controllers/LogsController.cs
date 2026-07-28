using Golinks.Application.Common;
using Golinks.Application.Features.Logs.Queries.GetAccessLogs;
using Golinks.Application.Requests;
using Golinks.Application.Responses;
using Golinks.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Golinks.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class LogsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Lists access logs with optional filters.
    /// </summary>
    /// <param name="params">Filter and pagination parameters.</param>
    /// <response code="200">Paginated list of access log entries.</response>
    /// <response code="401">The request is not authenticated.</response>
    [HttpGet(Name = "GetAccessLogs")]
    [ProducesResponseType(typeof(PagedResult<AccessLogResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Index([FromQuery] LogsParams @params)
    {
        var baseUrl = Url.Action(nameof(Index), "Logs", null, Request.Scheme);

        var toInclusive = @params.To.HasValue
            ? DateTime.SpecifyKind(@params.To.Value.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc)
            : (DateTime?)null;

        var result = await mediator.Send(new GetAccessLogsQuery(
            @params.PageNumber,
            @params.PageSize,
            baseUrl,
            @params.LinkId,
            @params.From?.Date,
            toInclusive,
            @params.BotFilter));

        return result.ToActionResult(this, Ok);
    }
}
