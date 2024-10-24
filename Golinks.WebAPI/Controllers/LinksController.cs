using AutoMapper;
using Golinks.Application.Contracts;
using Golinks.Application.Requests;
using Golinks.Application.ViewModel;
using Golinks.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Golinks.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class LinksController(ILinkService linkService, IMapper mapper) : ControllerBase
{
    private readonly ILinkService _linkSerice = linkService;
    private readonly IMapper _mapper = mapper;

    [HttpGet(Name = "GetAllLinks")]
    [ProducesResponseType(typeof(RestResponse<IEnumerable<LinkViewModel>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Index([FromQuery] LinkParams @params)
    {
        var data = await _linkSerice.FindAllAsync(@params.PageNumber, @params.PageSize);

        var result = RestResponse<IEnumerable<LinkViewModel>>.Success(_mapper.Map<IEnumerable<LinkViewModel>>(data));

        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = "GetLinkById")]
    [ProducesResponseType(typeof(RestResponse<LinkViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(RestResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var data = await _linkSerice.FindByIdAsync(id);

        if (data == null)
        {
            return BadRequest(RestResponse<object>.Error($"Link with ID {id} was not found."));
        }

        var result = RestResponse<LinkViewModel>.Success(_mapper.Map<LinkViewModel>(data));

        return Ok(result);
    }

    [HttpPost(Name = "CreateLink")]
    [ProducesResponseType(typeof(RestResponse<LinkViewModel>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(RestResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] LinkViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(RestResponse<object>.Error("The request is invalid."));
        }

        var linkInDb = await _linkSerice.FindOneAsync(f => f.Slug == model.Slug);

        if (linkInDb != null)
        {
            return BadRequest(RestResponse<object>.Error($"Slug \"{model.Slug}\" already exists."));
        }

        var link = _mapper.Map<Link>(model);

        await _linkSerice.CreateAsync(link);

        var result = RestResponse<LinkViewModel>.Success(_mapper.Map<LinkViewModel>(link));

        return CreatedAtAction(nameof(GetById), new { id = link.Id }, result);
    }

    [HttpPut("{id:guid}", Name = "UpdateLink")]
    [ProducesResponseType(typeof(RestResponse<LinkViewModel>), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(RestResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] LinkViewModel model)
    {
        var linkInDb = await _linkSerice.FindOneAsync(x => x.Slug == model.Slug);

        if (linkInDb?.Id != id)
        {
            return BadRequest(RestResponse<object>.Error("Slug already in use."));
        }

        linkInDb = await _linkSerice.FindByIdAsync(id);

        if (linkInDb == null)
        {
            return BadRequest(RestResponse<object>.Error($"Link with ID {id} was not found."));
        }

        var link = _mapper.Map(model, linkInDb);

        await _linkSerice.UpdateAsync(link);

        var result = RestResponse<LinkViewModel>.Success(_mapper.Map<LinkViewModel>(link));

        return AcceptedAtAction(nameof(GetById), new { id = link.Id }, result);
    }

    [HttpDelete("{id:guid}", Name = "DeleteLink")]
    [ProducesResponseType(typeof(RestResponse<object>), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(RestResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var linkInDb = await _linkSerice.FindByIdAsync(id);

        if (linkInDb == null)
        {
            return BadRequest(RestResponse<object>.Error($"Link with ID {id} was not found."));
        }

        await _linkSerice.DeleteAsync(linkInDb);

        var result = RestResponse<object>.Success(new { });

        return AcceptedAtAction(nameof(GetById), new { id = linkInDb.Id }, result);
    }
}
