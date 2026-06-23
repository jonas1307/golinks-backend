using Golinks.Application.Features.Links.Queries.GetMetrics;
using Golinks.Application.Requests;
using Golinks.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Golinks.WebAPI.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
[EnableRateLimiting("public")]
public class MetricsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Lists links along with their access metrics over a given period.
    /// </summary>
    [HttpGet(Name = "GetLinksWithMetrics")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> GetMetrics([FromQuery] LinkMetricParams @params)
    {
        var baseUrl = Url.Action(nameof(GetMetrics), "Metrics", null, Request.Scheme);
        var result = await mediator.Send(new GetMetricsQuery(@params.PageNumber, @params.PageSize, @params.MetricRange, baseUrl));
        return result.ToActionResult(this, Ok);
    }
}
