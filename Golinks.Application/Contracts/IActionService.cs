using Golinks.Application.Requests;
using Golinks.Application.ViewModel;

namespace Golinks.Application.Contracts;

public interface IActionService
{
    Task<IEnumerable<LinkMetricViewModel>> GetLinksWithMetrics(LinkMetricParams @params);
    Task<LinkViewModel> RegisterAccess(string slug);
}
