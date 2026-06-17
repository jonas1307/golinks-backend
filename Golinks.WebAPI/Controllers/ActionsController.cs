using Golinks.Application.Features.Actions.Commands.RegisterAccess;
using Golinks.Application.Features.Actions.Queries.GetLinksWithMetrics;
using Golinks.Application.Requests;
using Golinks.Application.ViewModel;
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
    [ProducesResponseType(typeof(RestResponse<LinkViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RestResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterAccess(string slug)
    {
        var result = await mediator.Send(new RegisterAccessCommand(slug));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [AllowAnonymous]
    [HttpGet("GetLinksWithMetrics", Name = "GetLinksWithMetrics")]
    [ProducesResponseType(typeof(RestResponse<IEnumerable<LinkMetricViewModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLinksWithMetrics([FromQuery] LinkMetricParams @params)
    {
        var baseUrl = Url.Action("GetLinksWithMetrics", "Actions", null, Request.Scheme);
        var result = await mediator.Send(new GetLinksWithMetricsQuery(@params.PageNumber, @params.PageSize, @params.MetricRange, baseUrl));
        return Ok(result);
    }
}
