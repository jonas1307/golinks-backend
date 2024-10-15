using AutoMapper;
using Golinks.Application.Requests;
using Golinks.Application.ViewModel;
using Golinks.Domain.Entities;
using Golinks.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Golinks.Backend.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class LinksController(ILinkRepository linkRepository, IMapper mapper) : Controller
{
    private readonly ILinkRepository _linkRepository = linkRepository;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LinkViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<LinkViewModel>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Index([FromQuery] LinkParams @params)
    {
        var data = await _linkRepository.FindAllWithPaginationAsync(@params.PageNumber, @params.PageSize);

        var links = _mapper.Map<IEnumerable<LinkViewModel>>(data);

        return Ok(links);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(LinkViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<LinkViewModel>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Index(Guid id)
    {
        var data = await _linkRepository.FindByIdAsync(id);

        if (data == null)
        {
            return BadRequest();
        }

        var link = _mapper.Map<LinkViewModel>(data);

        return Ok(link);
    }

    [HttpPost]
    [ProducesResponseType(typeof(LinkViewModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(LinkViewModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] LinkViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var linkInDb = await _linkRepository.FindOneAsync(f => f.Slug == model.Slug);

        if (linkInDb != null)
        {
            return BadRequest();
        }

        var link = _mapper.Map<Link>(model);

        await _linkRepository.CreateAsync(link);

        return StatusCode(201, link);
    }
}
