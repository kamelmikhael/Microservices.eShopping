namespace Ordering.SharedKernel;

public abstract class AggregateRoot : EntityBase
{
    private readonly List<IDomainEvent> _events = [];

    public IReadOnlyCollection<IDomainEvent> Events => [.. _events];

    public void ClearEvents() => _events.Clear();

    public void RaiseEvent(IDomainEvent domainEvent) => _events.Add(domainEvent);
}
