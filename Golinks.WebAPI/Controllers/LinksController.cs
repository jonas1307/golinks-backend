using Golinks.Application.Features.Links.Commands.CreateLink;
using Golinks.Application.Features.Links.Commands.DeleteLink;
using Golinks.Application.Features.Links.Commands.UpdateLink;
using Golinks.Application.Features.Links.Queries.GetAllLinks;
using Golinks.Application.Features.Links.Queries.GetLinkById;
using Golinks.Application.Features.Links.Queries.GetLinkQrCode;
using Golinks.Application.Requests;
using Golinks.Application.Responses;
using Golinks.WebAPI.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Golinks.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class LinksController(IMediator mediator, IConfiguration configuration) : ControllerBase
{
    /// <summary>
    /// Lists all links with pagination.
    /// </summary>
    /// <param name="params">Pagination parameters (page number and page size).</param>
    /// <response code="200">Paginated list of links.</response>
    /// <response code="401">The request is not authenticated.</response>
    [HttpGet(Name = "GetAllLinks")]
    [ProducesResponseType(typeof(LinkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Index([FromQuery] LinkParams @params)
    {
        var baseUrl = Url.Action(nameof(Index), "Links", null, Request.Scheme);
        var result = await mediator.Send(new GetAllLinksQuery(@params.PageNumber, @params.PageSize, baseUrl));
        return result.ToActionResult(this, Ok);
    }

    /// <summary>
    /// Gets a single link by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the link.</param>
    /// <response code="200">The requested link.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="404">No link was found with the given identifier.</response>
    [HttpGet("{id:guid}", Name = "GetLinkById")]
    [ProducesResponseType(typeof(LinkResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetLinkByIdQuery(id));
        return result.ToActionResult(this, Ok);
    }

    /// <summary>
    /// Creates a new link.
    /// </summary>
    /// <param name="model">The link data to create.</param>
    /// <response code="201">The link was created.</response>
    /// <response code="400">The request body failed validation.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated user lacks the required permission.</response>
    /// <response code="409">A link with the same slug already exists.</response>
    [HttpPost(Name = "CreateLink")]
    [PermissionRequirement("golinks:admin", AuthenticationSchemes = "Bearer", Policy = "PermissionPolicy")]
    [ProducesResponseType(typeof(LinkResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] LinkRequest model)
    {
        var result = await mediator.Send(new CreateLinkCommand(model));
        return result.ToActionResult(this, data => CreatedAtAction(nameof(GetById), new { id = data.Id }, data));
    }

    /// <summary>
    /// Updates an existing link.
    /// </summary>
    /// <param name="id">The unique identifier of the link to update.</param>
    /// <param name="model">The new link data.</param>
    /// <response code="202">The link was updated.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated user lacks the required permission.</response>
    /// <response code="404">No link was found with the given identifier.</response>
    /// <response code="409">Another link already uses the given slug.</response>
    [HttpPut("{id:guid}", Name = "UpdateLink")]
    [PermissionRequirement("golinks:admin", AuthenticationSchemes = "Bearer", Policy = "PermissionPolicy")]
    [ProducesResponseType(typeof(LinkResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] LinkRequest model)
    {
        var result = await mediator.Send(new UpdateLinkCommand(id, model));
        return result.ToActionResult(this, data => AcceptedAtAction(nameof(GetById), new { id = data.Id }, data));
    }

    /// <summary>
    /// Generates a PNG QR code that encodes the public redirect URL of the link.
    /// </summary>
    /// <param name="id">The unique identifier of the link.</param>
    /// <response code="200">The QR code image as a PNG.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="404">No link was found with the given identifier.</response>
    [HttpGet("{id:guid}/qrcode", Name = "GetLinkQrCode")]
    [Produces("image/png")]
    [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetQrCode(Guid id)
    {
        var baseUrl = configuration["PublicBaseUrl"] ?? $"{Request.Scheme}://{Request.Host}";
        var result = await mediator.Send(new GetLinkQrCodeQuery(id, baseUrl));
        return result.ToActionResult(this, bytes => File(bytes, "image/png"));
    }

    /// <summary>
    /// Deletes a link by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the link to delete.</param>
    /// <response code="204">The link was deleted.</response>
    /// <response code="401">The request is not authenticated.</response>
    /// <response code="403">The authenticated user lacks the required permission.</response>
    /// <response code="404">No link was found with the given identifier.</response>
    [HttpDelete("{id:guid}", Name = "DeleteLink")]
    [PermissionRequirement("golinks:admin", AuthenticationSchemes = "Bearer", Policy = "PermissionPolicy")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteLinkCommand(id));
        return result.ToActionResult(this, NoContent);
    }
}
