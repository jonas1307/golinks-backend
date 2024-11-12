using AutoMapper;
using Golinks.Application.Contracts;
using Golinks.Application.Requests;
using Golinks.Application.ViewModel;
using Golinks.Domain.Entities;
using Golinks.Repository.Contracts;

namespace Golinks.Application.Services;

public class ActionService(ILinkRepository linkRepository, IMetricRepository metricRepository, IMapper mapper) : IActionService
{
    private readonly ILinkRepository _linkRepository = linkRepository;
    private readonly IMetricRepository _metricRepository = metricRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<RestResponse<LinkViewModel>> RegisterAccess(string slug)
    {
        var link = await _linkRepository.FindOneAsync(x => x.Slug == slug);

        if (link == null) 
        {
            return RestResponse<LinkViewModel>.Error("No link was found with the given slug.");
        }

        link.RegisterUsage();

        await _linkRepository.UpdateAsync(link);

        var metric = new Metric { LinkId = link.Id };

        await _metricRepository.CreateAsync(metric);

        return RestResponse<LinkViewModel>.Success(_mapper.Map<LinkViewModel>(link));
    }

    public async Task<RestResponse<IEnumerable<LinkMetricViewModel>>> GetLinksWithMetrics(LinkMetricParams @params)
    {
        var links = await _linkRepository.AllLinksByMostPopularAsync(@params.PageNumber, @params.PageSize);

        var startDate = DateTime.UtcNow.Date.AddDays(@params.MetricRange * -1);
        
        var endDate = DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);

        var metrics = await _metricRepository.GetByLinks(links.Select(s => s.Id), startDate, endDate);

        var result = _mapper.Map<IEnumerable<LinkMetricViewModel>>(links);

        foreach (var metric in metrics)
        {
            var viewModel = _mapper.Map<MetricViewModel>(metric);

            var link = result.First(x => x.Id == metric.LinkId);

            link.Metrics.Add(viewModel);
        }

        return RestResponse<IEnumerable<LinkMetricViewModel>>.Success(result);
    }
}
