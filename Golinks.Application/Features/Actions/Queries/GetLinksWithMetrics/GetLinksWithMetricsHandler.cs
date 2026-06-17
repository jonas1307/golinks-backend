using AutoMapper;
using Golinks.Application.ViewModel;
using Golinks.Repository.Contracts;
using MediatR;

namespace Golinks.Application.Features.Actions.Queries.GetLinksWithMetrics;

public class GetLinksWithMetricsHandler(ILinkRepository linkRepository, IMetricRepository metricRepository, IMapper mapper) : IRequestHandler<GetLinksWithMetricsQuery, RestResponse<IEnumerable<LinkMetricViewModel>>>
{
    public async Task<RestResponse<IEnumerable<LinkMetricViewModel>>> Handle(GetLinksWithMetricsQuery request, CancellationToken cancellationToken)
    {
        var (links, totalItems) = await linkRepository.AllLinksByMostPopularAsync(request.PageNumber, request.PageSize);

        var startDate = DateTime.UtcNow.Date.AddDays(request.MetricRange * -1);
        var endDate = DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);

        var metrics = await metricRepository.GetByLinks(links.Select(x => x.Id), startDate, endDate);

        var result = mapper.Map<IEnumerable<LinkMetricViewModel>>(links);

        foreach (var metric in metrics)
        {
            var viewModel = mapper.Map<MetricViewModel>(metric);
            result.First(x => x.Id == metric.LinkId).Metrics.Add(viewModel);
        }

        return RestResponse<IEnumerable<LinkMetricViewModel>>.Success(result, request.BaseUrl, request.PageNumber, request.PageSize, totalItems);
    }
}
