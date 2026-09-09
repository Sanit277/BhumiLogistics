using BhumiLogistics.Domain.Events;

namespace BhumiLogistics.Domain.Common;

/// <summary>
/// Base class for all domain entities. Encapsulates identity and domain events,
/// enabling entities to raise events without depending on any messaging infrastructure.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
