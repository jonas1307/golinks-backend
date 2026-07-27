using Golinks.Application.Features.Metrics.Commands.BackfillUserAgents;
using Golinks.Application.Features.Metrics.Queries.GetDashboard;
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
public class DashboardController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Returns aggregated access metrics for the dashboard.
    /// </summary>
    /// <param name="days">Number of days to look back (default: 30).</param>
    /// <param name="linkId">Optional link filter.</param>
    /// <param name="excludeBots">When true, bot traffic is excluded from results.</param>
    /// <response code="200">Aggregated dashboard data.</response>
    /// <response code="401">The request is not authenticated.</response>
    [HttpGet(Name = "GetDashboard")]
    [ProducesResponseType(typeof(DashboardResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Get([FromQuery] int days = 30, [FromQuery] Guid? linkId = null, [FromQuery] bool excludeBots = false)
    {
        var result = await mediator.Send(new GetDashboardQuery(days, linkId, excludeBots));
        return result.ToActionResult(this, Ok);
    }

    /// <summary>
    /// Parses stored User-Agent strings and fills in device, browser and OS fields on existing metrics.
    /// </summary>
    /// <response code="200">Number of updated and skipped records.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated user lacks the required permission.</response>
    [HttpPost("backfill-ua", Name = "BackfillUserAgents")]
    [PermissionRequirement("golinks:admin", AuthenticationSchemes = "Bearer", Policy = "PermissionPolicy")]
    [ProducesResponseType(typeof(BackfillResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> BackfillUserAgents()
    {
        var result = await mediator.Send(new BackfillUserAgentsCommand());
        return result.ToActionResult(this, Ok);
    }
}
