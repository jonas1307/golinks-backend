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
public class LinksController(ILinkService linkService, IMapper mapper) : Controller
{
    private readonly ILinkService _linkSerice = linkService;
    private readonly IMapper _mapper = mapper;

    [HttpGet(Name = "GetAllLinks")]
    [ProducesResponseType(typeof(IEnumerable<LinkViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<LinkViewModel>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Index([FromQuery] LinkParams @params)
    {
        var data = await _linkSerice.FindAllAsync(@params.PageNumber, @params.PageSize);

        var links = _mapper.Map<IEnumerable<LinkViewModel>>(data);

        return Ok(links);
    }

    [HttpGet("{id:guid}", Name = "GetLinkById")]
    [ProducesResponseType(typeof(LinkViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<LinkViewModel>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var data = await _linkSerice.FindByIdAsync(id);

        if (data == null)
        {
            return BadRequest();
        }

        var link = _mapper.Map<LinkViewModel>(data);

        return Ok(link);
    }

    [HttpPost(Name = "CreateLink")]
    [ProducesResponseType(typeof(LinkViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(LinkViewModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] LinkViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var linkInDb = await _linkSerice.FindOneAsync(f => f.Slug == model.Slug);

        if (linkInDb != null)
        {
            return BadRequest();
        }

        var link = _mapper.Map<Link>(model);

        await _linkSerice.CreateAsync(link);

        return CreatedAtAction(nameof(GetById), new { id = link.Id }, link);
    }

    [HttpPut("{id:guid}", Name = "UpdateLink")]
    [ProducesResponseType(typeof(LinkViewModel), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, [FromBody] LinkViewModel model)
    {
        var linkInDb = await _linkSerice.FindOneAsync(x => x.Slug == model.Slug);

        if (linkInDb?.Id != id)
        {
            return BadRequest(new { Error = "Slug already in use" });
        }

        linkInDb = await _linkSerice.FindByIdAsync(id);

        if (linkInDb == null)
        {
            return BadRequest();
        }

        var link = _mapper.Map(model, linkInDb);

        await _linkSerice.UpdateAsync(link);

        return AcceptedAtAction(nameof(GetById), new { id = link.Id }, link);
    }

    [HttpDelete("{id:guid}", Name = "DeleteLink")]
    [ProducesResponseType(typeof(LinkViewModel), StatusCodes.Status202Accepted)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var linkInDb = await _linkSerice.FindByIdAsync(id);

        if (linkInDb == null)
        {
            return BadRequest();
        }

        await _linkSerice.DeleteAsync(linkInDb);

        return AcceptedAtAction(nameof(GetById), new { id = linkInDb.Id }, linkInDb);
    }
}
