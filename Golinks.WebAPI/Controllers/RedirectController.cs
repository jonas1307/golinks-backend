using Golinks.Application.Features.Links.Commands.TrackAccess;
using Golinks.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Golinks.WebAPI.Controllers;

[AllowAnonymous]
[ApiController]
[Route("")]
[Produces("application/json")]
[EnableRateLimiting("public")]
public class RedirectController(IMediator mediator) : ControllerBase
{
    [HttpGet("{slug}")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RedirectToUrl(string slug)
    {
        var userAgent = Request.Headers.UserAgent.ToString();
        var referrer = Request.Headers.Referer.ToString();
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

        var result = await mediator.Send(new TrackAccessCommand(slug, userAgent, referrer, ip));
        return result.ToActionResult(this, data => Redirect(data.Url));
    }
}
