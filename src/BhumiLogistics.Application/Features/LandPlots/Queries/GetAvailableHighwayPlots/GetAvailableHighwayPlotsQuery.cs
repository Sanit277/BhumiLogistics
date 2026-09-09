using BhumiLogistics.Domain.Enums;
using MediatR;

namespace BhumiLogistics.Application.Features.LandPlots.Queries.GetAvailableHighwayPlots;

/// <summary>
/// Returns all unleased plots, optionally filtered by highway frontage type —
/// the primary discovery query for corporate tenants.
/// </summary>
public sealed record GetAvailableHighwayPlotsQuery(HighwayType? HighwayFrontageType)
    : IRequest<IReadOnlyList<LandPlotSummaryDto>>;
