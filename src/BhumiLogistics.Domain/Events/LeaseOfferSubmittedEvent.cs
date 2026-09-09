namespace BhumiLogistics.Domain.Events;

public sealed record LeaseOfferSubmittedEvent(Guid LeaseOfferId, Guid LandPlotId, Guid TenantUserId) : DomainEvent;
