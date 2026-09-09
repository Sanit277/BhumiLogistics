using BhumiLogistics.Domain.Enums;

namespace BhumiLogistics.Application.Features.LandPlots.Queries.GetLandPlotById;

public sealed record LandPlotDto(
    Guid Id,
    string PlusCode,
    decimal SizeInBigha,
    decimal SizeInKattha,
    HighwayType HighwayFrontageType,
    bool IsLeased,
    Guid OwnerId);
