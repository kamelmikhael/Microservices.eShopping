using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Infrastructure.Persistence;
using Ordering.SharedKernel;
using System.Linq.Expressions;

namespace Ordering.Infrastructure.Repositories;

internal class ReadRepository<T>(OrderContext dbContext) : IReadRepository<T> where T : EntityBase
{
    private readonly OrderContext _dbContext = dbContext;
    protected readonly DbSet<T> _dbSet = dbContext.Set<T>();

    public async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null
        , Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null
        , string? includeString = null
        , bool disableTracking = true
        , CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (disableTracking) query = query.AsNoTracking();

        if(!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);

        if(predicate is not null) query = query.Where(predicate);

        if (orderBy is not null)
            return await orderBy(query).ToListAsync(cancellationToken);

        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate = null
        , Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null
        , List<Expression<Func<T, object>>>? includes = null
        , bool disableTracking = true
        , CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (disableTracking) query = query.AsNoTracking();

        if (includes is not null) query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (predicate is not null) query = query.Where(predicate);

        if (orderBy is not null)
            return await orderBy(query).ToListAsync(cancellationToken);

        return await _dbSet.ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> GetOneAsync(Expression<Func<T, bool>> predicate
        , bool disableTracking = true
        , CancellationToken cancellationToken = default)
    {
        var query = _dbSet.AsQueryable();

        if (disableTracking) query = query.AsNoTracking();

        return await _dbSet.Where(predicate).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
