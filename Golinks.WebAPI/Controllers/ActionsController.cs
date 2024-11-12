using Golinks.Application.Contracts;
using Golinks.Application.Requests;
using Golinks.Application.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Golinks.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class ActionsController(IActionService actionService) : ControllerBase
{
    private readonly IActionService _actionService = actionService;

    [AllowAnonymous]
    [HttpGet("RegisterAccess/{slug}", Name = "RegisterAccess")]
    [ProducesResponseType(typeof(RestResponse<LinkViewModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterAccess(string slug)
    {
        var response = await _actionService.RegisterAccess(slug);

        if (response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [AllowAnonymous]
    [HttpGet("GetLinksWithMetrics", Name = "GetLinksWithMetrics")]
    [ProducesResponseType(typeof(RestResponse<IEnumerable<LinkMetricViewModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetLinksWithMetrics([FromQuery] LinkMetricParams @params)
    {
        var links = await _actionService.GetLinksWithMetrics(@params);

        return Ok(links);
    }
}
