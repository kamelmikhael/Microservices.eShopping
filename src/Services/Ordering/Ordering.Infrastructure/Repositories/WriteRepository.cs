using Ordering.Application.Contracts.Persistence;
using Ordering.Infrastructure.Persistence;
using Ordering.SharedKernel;

namespace Ordering.Infrastructure.Repositories;

internal class WriteRepository<T>(OrderingDbContext dbContext) 
    : ReadRepository<T>(dbContext), IWriteRepository<T> where T : EntityBase
{
    public void Add(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
}
