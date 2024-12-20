﻿using System.Linq.Expressions;

namespace Golinks.Repository.Contracts;

public interface IBaseRepository<TDocument> where TDocument : class
{
    Task CreateAsync(TDocument entity);
    Task DeleteAsync(TDocument entity);
    Task<IList<TDocument>> FindAllAsync();
    Task<(IList<TDocument>, int)> FindAllAsync(int pageNumber, int pageSize);
    Task<IList<TDocument>> FindAllAsync(Expression<Func<TDocument, bool>> predicate);
    Task<TDocument> FindByIdAsync(Guid id);
    Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> predicate);
    Task UpdateAsync(TDocument entity);
}