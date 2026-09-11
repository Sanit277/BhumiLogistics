using BhumiLogistics.Domain.Common;
using BhumiLogistics.Domain.Enums;
using BhumiLogistics.Domain.Events;
using BhumiLogistics.Domain.Exceptions;
using BhumiLogistics.Domain.ValueObjects;

namespace BhumiLogistics.Domain.Entities;

/// <summary>
/// Aggregate root representing a highway-connected commercial land parcel
/// listed by a landowner for lease.
/// </summary>
public class LandPlot : BaseAuditableEntity
{
    public PlusCode PlusCode { get; private set; } = null!;
    public LandArea Area { get; private set; } = null!;
    public HighwayType HighwayFrontageType { get; private set; }
    public bool IsLeased { get; private set; }
    public Guid OwnerId { get; private set; }
    public decimal LatitudeCoordinate { get; private set; }
    public decimal LongitudeCoordinate { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public OwnershipVerification OwnershipVerification { get; private set; } = null!;

    private readonly List<LeaseOffer> _leaseOffers = new();
    public IReadOnlyCollection<LeaseOffer> LeaseOffers => _leaseOffers.AsReadOnly();

    private LandPlot() { } // EF Core

    private LandPlot(
        PlusCode plusCode,
        LandArea area,
        HighwayType highwayFrontageType,
        Guid ownerId,
        decimal latitude,
        decimal longitude,
        string description,
        OwnershipVerification ownershipVerification)
    {
        PlusCode = plusCode;
        Area = area;
        HighwayFrontageType = highwayFrontageType;
        OwnerId = ownerId;
        LatitudeCoordinate = latitude;
        LongitudeCoordinate = longitude;
        Description = description;
        OwnershipVerification = ownershipVerification;
        IsLeased = false;

        AddDomainEvent(new LandPlotListedEvent(Id, OwnerId));
    }

    public static LandPlot List(
        PlusCode plusCode,
        LandArea area,
        HighwayType highwayFrontageType,
        Guid ownerId,
        decimal latitude,
        decimal longitude,
        string description,
        OwnershipVerification ownershipVerification)
    {
        if (ownerId == Guid.Empty)
            throw new DomainException("A land plot must belong to a valid owner.");

        return new LandPlot(
            plusCode, area, highwayFrontageType, ownerId, latitude, longitude, description, ownershipVerification);
    }

    /// <summary>
    /// Registers a new lease offer against this plot. A plot already under
    /// an active lease cannot accept further offers.
    /// </summary>
    public void SubmitOffer(LeaseOffer offer)
    {
        if (IsLeased)
            throw new PlotAlreadyLeasedException(Id);

        _leaseOffers.Add(offer);
    }

    public void MarkAsLeased()
    {
        if (IsLeased)
            throw new PlotAlreadyLeasedException(Id);

        IsLeased = true;
    }

    public void ReleaseFromLease() => IsLeased = false;

    public void VerifyOwnership(Guid adminUserId, string? notes) =>
        OwnershipVerification.MarkVerified(adminUserId, notes);

    public void RejectOwnership(Guid adminUserId, string reason) =>
        OwnershipVerification.Reject(adminUserId, reason);
}