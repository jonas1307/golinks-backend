using Golinks.Domain.Entities;
using Golinks.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Golinks.Repository.Repositories;

public class LinkRepository(GolinksContext context) : BaseRepository<Link>(context), ILinkRepository
{
    public async Task<(IList<Link>, int)> AllLinksByMostPopularAsync(int pageNumber, int pageSize)
    {
        var query = _context.Links.OrderByDescending(x => x.TotalUsage);

        var totalItems = await query.CountAsync();
        var links = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return (links, totalItems);
    }

    public async Task<Link> IncrementUsageAsync(string slug)
    {
        await _context.Links
            .Where(x => x.Slug == slug)
            .ExecuteUpdateAsync(s => s.SetProperty(l => l.TotalUsage, l => l.TotalUsage + 1));

        return await _context.Links.FirstOrDefaultAsync(x => x.Slug == slug);
    }
}
