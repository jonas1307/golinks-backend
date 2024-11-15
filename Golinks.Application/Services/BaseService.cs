using Golinks.Application.Contracts;
using Golinks.Repository.Contracts;
using System.Linq.Expressions;

namespace Golinks.Application.Services;

public class BaseService<TEntity>(IBaseRepository<TEntity> repository) : IBaseService<TEntity> where TEntity : class
{
    private readonly IBaseRepository<TEntity> _repository = repository;

    public async Task CreateAsync(TEntity entity)
    {
        await _repository.CreateAsync(entity);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        await _repository.DeleteAsync(entity);
    }

    public async Task<IList<TEntity>> FindAllAsync()
    {
        return await _repository.FindAllAsync();
    }

    public async Task<(IList<TEntity>, int)> FindAllAsync(int pageNumber, int pageSize)
    {
        return await _repository.FindAllAsync(pageNumber, pageSize);
    }

    public async Task<IList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _repository.FindAllAsync(predicate);
    }

    public async Task<TEntity> FindByIdAsync(Guid id)
    {
        return await _repository.FindByIdAsync(id);
    }

    public async Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _repository.FindOneAsync(predicate);
    }

    public async Task UpdateAsync(TEntity entity)
    {
        await _repository.UpdateAsync(entity);
    }
}
