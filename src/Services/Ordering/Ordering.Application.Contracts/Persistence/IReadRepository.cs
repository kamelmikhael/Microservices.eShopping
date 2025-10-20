using Ordering.SharedKernel;
using System.Linq.Expressions;

namespace Ordering.Application.Contracts.Persistence;

public interface IReadRepository<T> where T : EntityBase
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null
        , Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null
        , string? includeString = null
        , bool disableTracking = true);
    Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null
        , Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null
        , List<Expression<Func<T, object>>>? includes = null
        , bool disableTracking = true);

    Task<T?> GetByIdAsync(int id);
    Task<T?> GetOneAsync(Expression<Func<T, bool>> predicate
        , bool disableTracking = true);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
