namespace BhumiLogistics.Domain.Events;

public abstract record DomainEvent
{
    public DateTimeOffset OccurredOnUtc { get; } = DateTimeOffset.UtcNow;
}
