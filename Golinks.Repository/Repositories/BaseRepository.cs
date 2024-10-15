using Golinks.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Golinks.Repository.Repositories;

public class BaseRepository<TDocument> : IBaseRepository<TDocument> where TDocument : class
{
    protected readonly GolinksContext _context;
    private readonly DbSet<TDocument> _dbSet;

    public BaseRepository(GolinksContext context)
    {
        _context = context;
        _dbSet = _context.Set<TDocument>();
    }

    public async Task<IList<TDocument>> FindAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IList<TDocument>> FindAllWithPaginationAsync(int pageNumber, int pageSize)
    {
        return await _dbSet.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize).ToListAsync();
    }

    public async Task<IList<TDocument>> FindByConditionAsync(Expression<Func<TDocument, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<TDocument> FindByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task CreateAsync(TDocument entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TDocument entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TDocument entity)
    {
        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }
}
