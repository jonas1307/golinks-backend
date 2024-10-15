using AutoMapper;
using Golinks.Application.ViewModel;
using Golinks.Domain.Entities;
using Golinks.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Golinks.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class MetricsController(IMetricRepository metricRepository, IMapper mapper) : Controller
{
    private readonly IMetricRepository _metricRepository = metricRepository;
    private readonly IMapper _mapper = mapper;

    [HttpGet]
    [ProducesResponseType(typeof(MetricViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MetricViewModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Index()
    {
        var data = await _metricRepository.FindAllAsync();

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
        
        await _metricRepository.CreateAsync(metric);

        return Ok(metric);
    }
}
