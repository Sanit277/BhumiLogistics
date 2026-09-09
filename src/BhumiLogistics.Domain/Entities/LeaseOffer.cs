using BhumiLogistics.Domain.Common;
using BhumiLogistics.Domain.Enums;
using BhumiLogistics.Domain.Events;
using BhumiLogistics.Domain.Exceptions;

namespace BhumiLogistics.Domain.Entities;

/// <summary>
/// Represents a lease proposal submitted by a corporate tenant against a
/// specific <see cref="LandPlot"/>.
/// </summary>
public class LeaseOffer : BaseAuditableEntity
{
    public Guid LandPlotId { get; private set; }
    public string TenantName { get; private set; } = string.Empty;
    public Guid TenantUserId { get; private set; }
    public decimal OfferedAmount { get; private set; }
    public int DurationInYears { get; private set; }
    public OfferStatus Status { get; private set; }
    public DateOnly ProposedStartDate { get; private set; }

    private LeaseOffer() { } // EF Core

    private LeaseOffer(
        Guid landPlotId,
        string tenantName,
        Guid tenantUserId,
        decimal offeredAmount,
        int durationInYears,
        DateOnly proposedStartDate)
    {
        LandPlotId = landPlotId;
        TenantName = tenantName;
        TenantUserId = tenantUserId;
        OfferedAmount = offeredAmount;
        DurationInYears = durationInYears;
        ProposedStartDate = proposedStartDate;
        Status = OfferStatus.Pending;

        AddDomainEvent(new LeaseOfferSubmittedEvent(Id, LandPlotId, TenantUserId));
    }

    public static LeaseOffer Create(
        Guid landPlotId,
        string tenantName,
        Guid tenantUserId,
        decimal offeredAmount,
        int durationInYears,
        DateOnly proposedStartDate)
    {
        if (offeredAmount <= 0)
            throw new DomainException("Offered lease amount must be greater than zero.");

        if (durationInYears is <= 0 or > 99)
            throw new DomainException("Lease duration must be between 1 and 99 years.");

        if (proposedStartDate < DateOnly.FromDateTime(DateTime.UtcNow))
            throw new DomainException("Proposed lease start date cannot be in the past.");

        return new LeaseOffer(landPlotId, tenantName, tenantUserId, offeredAmount, durationInYears, proposedStartDate);
    }

    public void Accept() => Status = OfferStatus.Accepted;
    public void Reject() => Status = OfferStatus.Rejected;
    public void Withdraw() => Status = OfferStatus.Withdrawn;
}
