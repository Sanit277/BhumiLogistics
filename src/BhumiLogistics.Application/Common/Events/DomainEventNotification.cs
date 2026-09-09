using BhumiLogistics.Domain.Events;
using MediatR;

namespace BhumiLogistics.Application.Common.Events;

/// <summary>
/// Wraps a pure Domain event so it can travel through the MediatR pipeline
/// without the Domain layer ever referencing MediatR.
/// </summary>
public sealed class DomainEventNotification<TDomainEvent> : INotification
    where TDomainEvent : DomainEvent
{
    public TDomainEvent DomainEvent { get; }

    public DomainEventNotification(TDomainEvent domainEvent) => DomainEvent = domainEvent;
}
