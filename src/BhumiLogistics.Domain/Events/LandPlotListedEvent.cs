namespace BhumiLogistics.Domain.Events;

public sealed record LandPlotListedEvent(Guid LandPlotId, Guid OwnerId) : DomainEvent;
