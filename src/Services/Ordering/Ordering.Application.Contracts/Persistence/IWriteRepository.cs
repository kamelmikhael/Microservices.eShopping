using Ordering.SharedKernel;

namespace Ordering.Application.Contracts.Persistence;

public interface IWriteRepository<T> : IReadRepository<T> where T : EntityBase
{
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
}
