using Golinks.Application.Features.Actions.Commands.RegisterAccess;
using Golinks.Application.Features.Actions.Queries.GetLinksWithMetrics;
using Golinks.Application.Requests;
using Golinks.Application.ViewModel;
using Golinks.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Golinks.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ActionsController(IMediator mediator) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("RegisterAccess/{slug}", Name = "RegisterAccess")]
    [ProducesResponseType(typeof(LinkViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegisterAccess(string slug)
    {
        var result = await mediator.Send(new RegisterAccessCommand(slug));
        return result.ToActionResult(this, Ok);
    }

    [AllowAnonymous]
    [HttpGet("GetLinksWithMetrics", Name = "GetLinksWithMetrics")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLinksWithMetrics([FromQuery] LinkMetricParams @params)
    {
        var baseUrl = Url.Action("GetLinksWithMetrics", "Actions", null, Request.Scheme);
        var result = await mediator.Send(new GetLinksWithMetricsQuery(@params.PageNumber, @params.PageSize, @params.MetricRange, baseUrl));
        return result.ToActionResult(this, Ok);
    }
}
