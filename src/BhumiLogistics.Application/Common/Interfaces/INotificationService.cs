namespace BhumiLogistics.Application.Common.Interfaces;

/// <summary>
/// Abstraction for outbound alerts (e.g. notifying a landowner that a new
/// lease offer has been submitted, or a milestone payment threshold is hit).
/// </summary>
public interface INotificationService
{
    Task SendLeaseOfferAlertAsync(string recipientEmail, string plotReference, decimal offeredAmount, CancellationToken cancellationToken);
}
