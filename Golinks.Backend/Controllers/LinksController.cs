using AutoMapper;
using Golinks.Application.ViewModel;
using Golinks.Domain.Entities;
using Golinks.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Golinks.Backend.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class LinksController : Controller
{
    private readonly ILinkRepository _linkRepository;
    private readonly IMapper _mapper;

    public LinksController(ILinkRepository linkRepository, IMapper mapper)
    {
        _linkRepository = linkRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<LinkViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<LinkViewModel>), StatusCodes.Status404NotFound)]
    public IActionResult Index()
    {
        var data = _linkRepository.AsQueryable();

        var links = _mapper.Map<LinkViewModel>(data);

        return Ok(links);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(LinkViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IEnumerable<LinkViewModel>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Index(string id)
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

        var linkInDb = await _linkRepository.FindAsync(f => f.Alias == model.Alias);

        if (linkInDb != null)
        {
            return BadRequest();
        }

        var link = _mapper.Map<Link>(model);

        await _linkRepository.InsertAsync(link);

        return StatusCode(201, link);
    }
}
