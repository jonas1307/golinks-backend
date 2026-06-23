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
    /// <summary>
    /// Redirects to the original URL for the given slug and tracks the access.
    /// </summary>
    /// <remarks>
    /// This is the core public endpoint of the shortener. Each call increments the
    /// link usage counter and records an access metric (user agent, referrer and a
    /// hashed IP). It is rate limited per client IP.
    /// </remarks>
    /// <param name="slug">The short slug to resolve.</param>
    /// <response code="302">Redirects to the original URL.</response>
    /// <response code="404">No link was found with the given slug.</response>
    /// <response code="410">The link exists but has expired.</response>
    [HttpGet("{slug}")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status410Gone)]
    public async Task<IActionResult> RedirectToUrl(string slug)
    {
        var userAgent = Request.Headers.UserAgent.ToString();
        var referrer = Request.Headers.Referer.ToString();
        var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

        var result = await mediator.Send(new TrackAccessCommand(slug, userAgent, referrer, ip));
        return result.ToActionResult(this, data => Redirect(data.Url));
    }
}
