using AutoMapper;
using Golinks.Application.Contracts;
using Golinks.Application.ViewModel;
using Golinks.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Golinks.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class MetricsController(IMetricService metricService, IMapper mapper) : Controller
{
    private readonly IMetricService _metricService = metricService;
    private readonly IMapper _mapper = mapper;

    [HttpGet(Name = "GetAllMetrics")]
    [ProducesResponseType(typeof(MetricViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MetricViewModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Index()
    {
        var data = await _metricService.FindAllAsync();

        var metrics = _mapper.Map<IEnumerable<MetricViewModel>>(data);

        return Ok(metrics);
    }

    [HttpPost(Name = "CreateNewMetric")]
    [ProducesResponseType(typeof(MetricViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(MetricViewModel), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] MetricViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest();
        }

        var metric = _mapper.Map<Metric>(model);
        
        await _metricService.CreateAsync(metric);

        return Ok(metric);
    }
}
