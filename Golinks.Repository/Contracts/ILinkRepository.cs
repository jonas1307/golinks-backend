using Golinks.Domain.Entities;

namespace Golinks.Repository.Contracts;

public interface ILinkRepository : IBaseRepository<Link>
{
    Task<IList<Link>> AllLinksByMostPopularAsync(int pageNumber, int pageSize);
}