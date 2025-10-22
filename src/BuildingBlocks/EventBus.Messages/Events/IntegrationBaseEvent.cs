namespace EventBus.Messages.Events;

public class IntegrationBaseEvent
{
    public Guid Id { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public IntegrationBaseEvent()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
    }

    public IntegrationBaseEvent(Guid id, DateTime createdAt)
    {
        Id = id;
        CreatedAt = createdAt;
    }
}
