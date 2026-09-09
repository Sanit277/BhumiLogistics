using BhumiLogistics.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace BhumiLogistics.Infrastructure.Services;

/// <summary>
/// Placeholder implementation of the notification port. Swap this for a
/// real provider (SendGrid, SES, Twilio) without touching Application code.
/// </summary>
public class EmailNotificationService : INotificationService
{
    private readonly ILogger<EmailNotificationService> _logger;

    public EmailNotificationService(ILogger<EmailNotificationService> logger) => _logger = logger;

    public Task SendLeaseOfferAlertAsync(
        string recipientEmail, string plotReference, decimal offeredAmount, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "[MOCK EMAIL] To: {Recipient} | New lease offer of {Amount:C} received for plot {PlotReference}.",
            recipientEmail, offeredAmount, plotReference);

        return Task.CompletedTask;
    }
}
