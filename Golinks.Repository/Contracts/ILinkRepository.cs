using Golinks.Domain.Entities;

namespace Golinks.Repository.Contracts;

public interface ILinkRepository : IBaseRepository<Link>
{
    Task<(IList<Link>, int)> AllLinksByMostPopularAsync(int pageNumber, int pageSize);
    Task<Link> IncrementUsageAsync(string slug);
}