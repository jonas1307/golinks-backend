using Golinks.Application.Features.Links.Commands.CreateLink;
using Golinks.Application.Features.Links.Commands.DeleteLink;
using Golinks.Application.Features.Links.Commands.RegisterAccess;
using Golinks.Application.Features.Links.Commands.UpdateLink;
using Golinks.Application.Features.Links.Queries.GetAllLinks;
using Golinks.Application.Features.Links.Queries.GetLinkById;
using Golinks.Application.Features.Links.Queries.GetMetrics;
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
public class LinksController(IMediator mediator) : ControllerBase
{
    [HttpGet(Name = "GetAllLinks")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Index([FromQuery] LinkParams @params)
    {
        var baseUrl = Url.Action(nameof(Index), "Links", null, Request.Scheme);
        var result = await mediator.Send(new GetAllLinksQuery(@params.PageNumber, @params.PageSize, baseUrl));
        return result.ToActionResult(this, Ok);
    }

    [HttpGet("{id:guid}", Name = "GetLinkById")]
    [ProducesResponseType(typeof(LinkViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetLinkByIdQuery(id));
        return result.ToActionResult(this, Ok);
    }

    [HttpPost(Name = "CreateLink")]
    [PermissionRequirement("golinks:admin", AuthenticationSchemes = "Bearer", Policy = "PermissionPolicy")]
    [ProducesResponseType(typeof(LinkViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] LinkViewModel model)
    {
        var result = await mediator.Send(new CreateLinkCommand(model));
        return result.ToActionResult(this, data => CreatedAtAction(nameof(GetById), new { id = data.Id }, data));
    }

    [HttpPut("{id:guid}", Name = "UpdateLink")]
    [PermissionRequirement("golinks:admin", AuthenticationSchemes = "Bearer", Policy = "PermissionPolicy")]
    [ProducesResponseType(typeof(LinkViewModel), StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] LinkViewModel model)
    {
        var result = await mediator.Send(new UpdateLinkCommand(id, model));
        return result.ToActionResult(this, data => AcceptedAtAction(nameof(GetById), new { id = data.Id }, data));
    }

    [HttpDelete("{id:guid}", Name = "DeleteLink")]
    [PermissionRequirement("golinks:admin", AuthenticationSchemes = "Bearer", Policy = "PermissionPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteLinkCommand(id));
        return result.ToActionResult(this, NoContent);
    }

    [AllowAnonymous]
    [HttpPost("register-access/{slug}", Name = "RegisterAccess")]
    [ProducesResponseType(typeof(LinkViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RegisterAccess(string slug)
    {
        var result = await mediator.Send(new RegisterAccessCommand(slug));
        return result.ToActionResult(this, Ok);
    }

    [AllowAnonymous]
    [HttpGet("metrics", Name = "GetLinksWithMetrics")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMetrics([FromQuery] LinkMetricParams @params)
    {
        var baseUrl = Url.Action(nameof(GetMetrics), "Links", null, Request.Scheme);
        var result = await mediator.Send(new GetMetricsQuery(@params.PageNumber, @params.PageSize, @params.MetricRange, baseUrl));
        return result.ToActionResult(this, Ok);
    }
}
