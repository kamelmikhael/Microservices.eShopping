using Ordering.SharedKernel;

namespace Ordering.Application.Contracts.Persistence;

public interface IWriteRepository<T> : IReadRepository<T> where T : EntityBase
{
    void Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}
