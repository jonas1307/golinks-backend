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

    public async Task<LinkViewModel> RegisterAccess(string slug)
    {
        var link = await _linkRepository.FindOneAsync(x => x.Slug == slug)
            ?? throw new Exception("Invalid Slug");

        var metric = new Metric { LinkId = link.Id };

        await _metricRepository.CreateAsync(metric);

        return _mapper.Map<LinkViewModel>(link);
    }

    public async Task<IEnumerable<LinkMetricViewModel>> GetLinksWithMetrics(LinkMetricParams @params)
    {
        throw new NotImplementedException();
    }
}
