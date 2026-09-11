using BhumiLogistics.Application.Common.Interfaces;
using MediatR;

namespace BhumiLogistics.Application.Features.LandPlots.Queries.GetAvailableHighwayPlots;

public class GetAvailableHighwayPlotsQueryHandler
    : IRequestHandler<GetAvailableHighwayPlotsQuery, IReadOnlyList<LandPlotSummaryDto>>
{
    private readonly ILandPlotRepository _landPlotRepository;

    public GetAvailableHighwayPlotsQueryHandler(ILandPlotRepository landPlotRepository) =>
        _landPlotRepository = landPlotRepository;

    public async Task<IReadOnlyList<LandPlotSummaryDto>> Handle(
        GetAvailableHighwayPlotsQuery request, CancellationToken cancellationToken)
    {
        var plots = await _landPlotRepository.GetAvailablePlotsByHighwayTypeAsync(
            request.HighwayFrontageType, cancellationToken);

        return plots
            .Where(p => p.OwnershipVerification.Status == Domain.Enums.OwnershipVerificationStatus.Verified)
            .Select(p => new LandPlotSummaryDto(p.Id, p.PlusCode.Value, p.Area.ToTotalKattha(), p.HighwayFrontageType))
            .ToList();
    }
}