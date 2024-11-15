using System.Linq.Expressions;

namespace Golinks.Application.Contracts;

public interface IBaseService<TEntity> where TEntity : class
{
    Task CreateAsync(TEntity entity);
    Task DeleteAsync(TEntity entity);
    Task<IList<TEntity>> FindAllAsync();
    Task<(IList<TEntity>, int)> FindAllAsync(int pageNumber, int pageSize);
    Task<IList<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> FindByIdAsync(Guid id);
    Task<TEntity> FindOneAsync(Expression<Func<TEntity, bool>> predicate);
    Task UpdateAsync(TEntity entity);
}