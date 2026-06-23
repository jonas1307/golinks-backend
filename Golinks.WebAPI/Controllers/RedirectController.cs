using Golinks.Application.Features.Links.Commands.TrackAccess;
using Golinks.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Golinks.WebAPI.Controllers;

[AllowAnonymous]
[ApiController]
[Route("")]
[Produces("application/json")]
public class RedirectController(IMediator mediator) : ControllerBase
{
    [HttpGet("{slug}")]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RedirectToUrl(string slug)
    {
        var result = await mediator.Send(new TrackAccessCommand(slug));
        return result.ToActionResult(this, data => Redirect(data.Url));
    }
}
