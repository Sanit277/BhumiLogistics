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
    public LandUseDeclaration LandUseDeclaration { get; private set; } = null!;
    public decimal FrontageLengthInMeters { get; private set; }

    /// <summary>
    /// Setback distance in effect at the time this plot was listed. Snapshotted
    /// rather than looked up live, so a future change to the admin-editable
    /// standard doesn't silently alter the buildable area of existing listings.
    /// </summary>
    public decimal SetbackDistanceInMeters { get; private set; }

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
        OwnershipVerification ownershipVerification,
        LandUseDeclaration landUseDeclaration,
        decimal frontageLengthInMeters,
        decimal setbackDistanceInMeters)
    {
        PlusCode = plusCode;
        Area = area;
        HighwayFrontageType = highwayFrontageType;
        OwnerId = ownerId;
        LatitudeCoordinate = latitude;
        LongitudeCoordinate = longitude;
        Description = description;
        OwnershipVerification = ownershipVerification;
        LandUseDeclaration = landUseDeclaration;
        FrontageLengthInMeters = frontageLengthInMeters;
        SetbackDistanceInMeters = setbackDistanceInMeters;
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
        OwnershipVerification ownershipVerification,
        LandUseDeclaration landUseDeclaration,
        decimal frontageLengthInMeters,
        decimal setbackDistanceInMeters)
    {
        if (ownerId == Guid.Empty)
            throw new DomainException("A land plot must belong to a valid owner.");

        if (frontageLengthInMeters < 0)
            throw new DomainException("Frontage length cannot be negative.");

        return new LandPlot(
            plusCode, area, highwayFrontageType, ownerId, latitude, longitude, description,
            ownershipVerification, landUseDeclaration, frontageLengthInMeters, setbackDistanceInMeters);
    }

    /// <summary>
    /// The portion of the plot actually usable for construction, after subtracting
    /// the highway right-of-way/setback strip along the frontage. Never negative —
    /// a plot smaller than its setback requirement simply has zero buildable area.
    /// </summary>
    public decimal BuildableAreaInKattha
    {
        get
        {
            var setbackAreaInSquareMeters = FrontageLengthInMeters * SetbackDistanceInMeters;
            var setbackAreaInKattha = setbackAreaInSquareMeters / LandArea.SquareMetersPerKattha;
            var buildable = Area.ToTotalKattha() - setbackAreaInKattha;
            return buildable < 0 ? 0 : buildable;
        }
    }

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