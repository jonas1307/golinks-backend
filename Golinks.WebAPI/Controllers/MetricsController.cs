using Golinks.Application.Features.Links.Queries.GetMetrics;
using Golinks.Application.Requests;
using Golinks.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Golinks.WebAPI.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class MetricsController(IMediator mediator) : ControllerBase
{
    [HttpGet(Name = "GetLinksWithMetrics")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMetrics([FromQuery] LinkMetricParams @params)
    {
        var baseUrl = Url.Action(nameof(GetMetrics), "Metrics", null, Request.Scheme);
        var result = await mediator.Send(new GetMetricsQuery(@params.PageNumber, @params.PageSize, @params.MetricRange, baseUrl));
        return result.ToActionResult(this, Ok);
    }
}
