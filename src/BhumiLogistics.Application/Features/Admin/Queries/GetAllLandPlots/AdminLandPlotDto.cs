using BhumiLogistics.Domain.Enums;

namespace BhumiLogistics.Application.Features.Admin.Queries.GetAllLandPlots;

public sealed record AdminLandPlotDto(
    Guid Id,
    string PlusCode,
    decimal TotalAreaInKattha,
    decimal BuildableAreaInKattha,
    HighwayType HighwayFrontageType,
    bool IsLeased,
    Guid OwnerId,
    int OfferCount,
    OwnershipVerificationStatus OwnershipStatus,
    string RegisteredOwnerName,
    LandTenureType TenureType,
    bool MohiTenancyDeclared,
    LandUseClassification LandUseClassification,
    decimal FrontageLengthInMeters,
    decimal SetbackDistanceInMeters,
    DateTimeOffset CreatedAtUtc);