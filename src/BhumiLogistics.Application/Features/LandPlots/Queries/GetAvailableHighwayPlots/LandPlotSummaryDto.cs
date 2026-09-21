using BhumiLogistics.Domain.Enums;

namespace BhumiLogistics.Application.Features.LandPlots.Queries.GetAvailableHighwayPlots;

public sealed record LandPlotSummaryDto(
    Guid Id,
    string PlusCode,
    decimal TotalAreaInKattha,
    decimal BuildableAreaInKattha,
    HighwayType HighwayFrontageType);