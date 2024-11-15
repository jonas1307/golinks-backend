using Golinks.Application.Requests;
using Golinks.Application.ViewModel;

namespace Golinks.Application.Contracts;

public interface IActionService
{
    Task<RestResponse<IEnumerable<LinkMetricViewModel>>> GetLinksWithMetrics(LinkMetricParams @params, string baseUrl);
    Task<RestResponse<LinkViewModel>> RegisterAccess(string slug);
}
