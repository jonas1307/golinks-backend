using System.Linq.Expressions;

namespace Golinks.Repository.Contracts;

public interface IRepositoryBase<TDocument> where TDocument : class
{
    IQueryable<TDocument> AsQueryable();
    Task DeleteAsync(Expression<Func<TDocument, bool>> filterExpression);
    Task DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
    IEnumerable<TDocument> FilterBy(Expression<Func<TDocument, bool>> filterExpression);
    IEnumerable<TProjected> FilterBy<TProjected>(Expression<Func<TDocument, bool>> filterExpression, Expression<Func<TDocument, TProjected>> projectionExpression);
    Task<TDocument> FindAsync(Expression<Func<TDocument, bool>> filterExpression);
    Task<TDocument> FindByIdAsync(string id);
    Task InsertAsync(TDocument document);
    Task InsertManyAsync(ICollection<TDocument> documents);
}