using AutoMapper;
using Golinks.Application.ViewModel;
using Golinks.Domain.Entities;
using Golinks.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Golinks.Backend.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class MetricsController : Controller
{
    private readonly IMetricRepository _metricRepository;
    private readonly IMapper _mapper;

    public MetricsController(IMetricRepository metricRepository, IMapper mapper)
    {
        _metricRepository = metricRepository;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(MetricViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MetricViewModel), StatusCodes.Status400BadRequest)]
    public IActionResult Index()
    {
        var data = _metricRepository.AsQueryable().ToList();

        var metrics = _mapper.Map<IEnumerable<MetricViewModel>>(data);

        return Ok(metrics);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MetricViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MetricViewModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] MetricViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var metric = _mapper.Map<Metric>(model);
        
        await _metricRepository.InsertAsync(metric);

        return Ok(metric);
    }
}
