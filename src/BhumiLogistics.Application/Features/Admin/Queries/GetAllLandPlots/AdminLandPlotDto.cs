using BhumiLogistics.Domain.Enums;

namespace BhumiLogistics.Application.Features.Admin.Queries.GetAllLandPlots;

public sealed record AdminLandPlotDto(
    Guid Id,
    string PlusCode,
    decimal TotalAreaInKattha,
    HighwayType HighwayFrontageType,
    bool IsLeased,
    Guid OwnerId,
    int OfferCount,
    DateTimeOffset CreatedAtUtc);