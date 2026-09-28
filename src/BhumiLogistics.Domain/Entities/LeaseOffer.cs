using BhumiLogistics.Domain.Common;
using BhumiLogistics.Domain.Enums;
using BhumiLogistics.Domain.Events;
using BhumiLogistics.Domain.Exceptions;

namespace BhumiLogistics.Domain.Entities;

public class LeaseOffer : BaseAuditableEntity
{
    public Guid LandPlotId { get; private set; }
    public string TenantName { get; private set; } = string.Empty;
    public Guid TenantUserId { get; private set; }
    public decimal OfferedAmount { get; private set; }
    public int DurationInYears { get; private set; }
    public OfferStatus Status { get; private set; }
    public DateOnly ProposedStartDate { get; private set; }

    public MalpotRegistrationStatus MalpotRegistrationStatus { get; private set; }
    public string? RegisteredDeedReferenceNumber { get; private set; }
    public DateOnly? RegistrationDate { get; private set; }
    public DateTimeOffset? RegisteredAtUtc { get; private set; }
    public Guid? RegisteredByUserId { get; private set; }

    public bool RequiresGovernmentRegistration => DurationInYears >= 1;

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
        MalpotRegistrationStatus = MalpotRegistrationStatus.NotRegistered;

        AddDomainEvent(new LeaseOfferSubmittedEvent(Id, LandPlotId, TenantUserId));
    }

    /// <summary>
    /// maximumDurationInYears is supplied by the caller (SubmitLeaseOfferCommandHandler),
    /// since the applicable cap depends on the tenant's company type — 35 years domestically,
    /// up to 75 for an approved foreign-invested tenant. See User.MaximumLeaseDurationInYearsAsTenant.
    /// </summary>
    public static LeaseOffer Create(
        Guid landPlotId,
        string tenantName,
        Guid tenantUserId,
        decimal offeredAmount,
        int durationInYears,
        DateOnly proposedStartDate,
        int maximumDurationInYears)
    {
        if (offeredAmount <= 0)
            throw new DomainException("Offered lease amount must be greater than zero.");

        if (durationInYears is <= 0 || durationInYears > maximumDurationInYears)
            throw new DomainException(
                $"Lease duration must be between 1 and {maximumDurationInYears} years for this tenant type.");

        if (proposedStartDate < DateOnly.FromDateTime(DateTime.UtcNow))
            throw new DomainException("Proposed lease start date cannot be in the past.");

        return new LeaseOffer(landPlotId, tenantName, tenantUserId, offeredAmount, durationInYears, proposedStartDate);
    }

    public void Accept() => Status = OfferStatus.Accepted;
    public void Reject() => Status = OfferStatus.Rejected;
    public void Withdraw() => Status = OfferStatus.Withdrawn;

    public void MarkPendingRegistration()
    {
        if (Status != OfferStatus.Accepted)
            throw new DomainException("Only an accepted lease offer can be marked pending registration.");

        if (MalpotRegistrationStatus == MalpotRegistrationStatus.Registered)
            throw new DomainException("This lease has already been registered.");

        MalpotRegistrationStatus = MalpotRegistrationStatus.PendingRegistration;
    }

    public void MarkRegistered(string deedReferenceNumber, DateOnly registrationDate, Guid registeredByUserId)
    {
        if (Status != OfferStatus.Accepted)
            throw new DomainException("Only an accepted lease offer can be registered.");

        if (string.IsNullOrWhiteSpace(deedReferenceNumber))
            throw new DomainException("A registered deed reference number is required.");

        if (registrationDate > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new DomainException("Registration date cannot be in the future.");

        MalpotRegistrationStatus = MalpotRegistrationStatus.Registered;
        RegisteredDeedReferenceNumber = deedReferenceNumber;
        RegistrationDate = registrationDate;
        RegisteredAtUtc = DateTimeOffset.UtcNow;
        RegisteredByUserId = registeredByUserId;
    }
}