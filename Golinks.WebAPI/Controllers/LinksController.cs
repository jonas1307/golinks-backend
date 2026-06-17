using Golinks.Application.Features.Links.Commands.CreateLink;
using Golinks.Application.Features.Links.Commands.DeleteLink;
using Golinks.Application.Features.Links.Commands.UpdateLink;
using Golinks.Application.Features.Links.Queries.GetAllLinks;
using Golinks.Application.Features.Links.Queries.GetLinkById;
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
    [ProducesResponseType(typeof(RestResponse<IEnumerable<LinkViewModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Index([FromQuery] LinkParams @params)
    {
        var baseUrl = Url.Action("Index", "Links", null, Request.Scheme);
        var result = await mediator.Send(new GetAllLinksQuery(@params.PageNumber, @params.PageSize, baseUrl));
        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = "GetLinkById")]
    [ProducesResponseType(typeof(RestResponse<LinkViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RestResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetLinkByIdQuery(id));
        return result.IsSuccess ? Ok(result) : BadRequest(result);
    }

    [HttpPost(Name = "CreateLink")]
    [PermissionRequirement("golinks:admin", AuthenticationSchemes = "Bearer", Policy = "PermissionPolicy")]
    [ProducesResponseType(typeof(RestResponse<LinkViewModel>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RestResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] LinkViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(RestResponse<object>.Error("The request is invalid."));

        var result = await mediator.Send(new CreateLinkCommand(model));

        if (!result.IsSuccess)
            return BadRequest(result);

        return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, result);
    }

    [HttpPut("{id:guid}", Name = "UpdateLink")]
    [PermissionRequirement("golinks:admin", AuthenticationSchemes = "Bearer", Policy = "PermissionPolicy")]
    [ProducesResponseType(typeof(RestResponse<LinkViewModel>), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(RestResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] LinkViewModel model)
    {
        var result = await mediator.Send(new UpdateLinkCommand(id, model));
        return result.IsSuccess ? AcceptedAtAction(nameof(GetById), new { id }, result) : BadRequest(result);
    }

    [HttpDelete("{id:guid}", Name = "DeleteLink")]
    [PermissionRequirement("golinks:admin", AuthenticationSchemes = "Bearer", Policy = "PermissionPolicy")]
    [ProducesResponseType(typeof(RestResponse<object>), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(RestResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteLinkCommand(id));
        return result.IsSuccess ? AcceptedAtAction(nameof(GetById), new { id }, result) : BadRequest(result);
    }
}
