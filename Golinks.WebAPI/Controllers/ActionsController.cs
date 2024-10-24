using Golinks.Application.Contracts;
using Golinks.Application.Requests;
using Golinks.Application.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace Golinks.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ActionsController(IActionService actionService) : ControllerBase
{
    private readonly IActionService _actionService = actionService;

    [HttpGet("RegisterAccess/{slug}", Name = "RegisterAccess")]
    [ProducesResponseType(typeof(RestResponse<LinkViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterAccess(string slug)
    {
        var link = await _actionService.RegisterAccess(slug);

        return Ok(link);
    }

    [HttpGet("GetLinksWithMetrics", Name = "GetLinksWithMetrics")]
    [ProducesResponseType(typeof(RestResponse<IEnumerable<LinkMetricViewModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLinksWithMetrics([FromQuery] LinkMetricParams @params)
    {
        var links = await _actionService.GetLinksWithMetrics(@params);

        return Ok(links);
    }
}
