using BhumiLogistics.Application.Common.Events;
using BhumiLogistics.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BhumiLogistics.Infrastructure.Persistence.Interceptors;

/// <summary>
/// After EF Core successfully saves changes, collects domain events raised
/// by tracked entities during this unit of work and publishes them via
/// MediatR — decoupling side effects (notifications, etc.) from command handlers.
/// </summary>
public class DispatchDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IPublisher _publisher;

    public DispatchDomainEventsInterceptor(IPublisher publisher) => _publisher = publisher;

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        if (eventData.Context is not null)
            await DispatchDomainEvents(eventData.Context);

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEvents(DbContext context)
    {
        var entitiesWithEvents = context.ChangeTracker
            .Entries<BaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToList();

        var domainEvents = entitiesWithEvents.SelectMany(e => e.DomainEvents).ToList();
        entitiesWithEvents.ForEach(e => e.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
        {
            var wrapperType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            var notification = (INotification)Activator.CreateInstance(wrapperType, domainEvent)!;
            await _publisher.Publish(notification);
        }
    }
}
