using BhumiLogistics.Application.Common.Events;
using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Events;
using MediatR;

namespace BhumiLogistics.Application.Features.LeaseOffers.EventHandlers;

/// <summary>
/// Reacts to a LeaseOfferSubmittedEvent by alerting the landowner —
/// fully decoupled from the command handler that created the offer.
/// </summary>
public class LeaseOfferSubmittedEventHandler
    : INotificationHandler<DomainEventNotification<LeaseOfferSubmittedEvent>>
{
    private readonly INotificationService _notificationService;
    private readonly ILandPlotRepository _landPlotRepository;

    public LeaseOfferSubmittedEventHandler(
        INotificationService notificationService, ILandPlotRepository landPlotRepository)
    {
        _notificationService = notificationService;
        _landPlotRepository = landPlotRepository;
    }

    public async Task Handle(
        DomainEventNotification<LeaseOfferSubmittedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var landPlot = await _landPlotRepository.GetByIdAsync(domainEvent.LandPlotId, cancellationToken);

        if (landPlot is null) return;

        var offer = landPlot.LeaseOffers.First(o => o.Id == domainEvent.LeaseOfferId);

        await _notificationService.SendLeaseOfferAlertAsync(
            recipientEmail: "owner-notification-placeholder@bhumilogistics.com",
            plotReference: landPlot.PlusCode.Value,
            offeredAmount: offer.OfferedAmount,
            cancellationToken);
    }
}
