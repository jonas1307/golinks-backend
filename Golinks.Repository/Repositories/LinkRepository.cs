using Golinks.Domain.Entities;
using Golinks.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Repository.Repositories;

public class LinkRepository(GolinksContext context) : BaseRepository<Link>(context), ILinkRepository
{
    public async Task<IList<Link>> AllLinksByMostPopularAsync(int pageNumber, int pageSize)
    {
        return await _context.Links.OrderByDescending(x => x.TotalUsage).Skip((pageNumber - 1) * pageSize)
        .Take(pageSize).ToListAsync();
    }
}
