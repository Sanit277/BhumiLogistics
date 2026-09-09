using BhumiLogistics.Application.Common.Interfaces;
using MediatR;

namespace BhumiLogistics.Application.Features.LandPlots.Queries.GetLandPlotById;

public class GetLandPlotByIdQueryHandler : IRequestHandler<GetLandPlotByIdQuery, LandPlotDto?>
{
    private readonly ILandPlotRepository _landPlotRepository;

    public GetLandPlotByIdQueryHandler(ILandPlotRepository landPlotRepository) =>
        _landPlotRepository = landPlotRepository;

    public async Task<LandPlotDto?> Handle(GetLandPlotByIdQuery request, CancellationToken cancellationToken)
    {
        var plot = await _landPlotRepository.GetByIdAsync(request.Id, cancellationToken);

        return plot is null
            ? null
            : new LandPlotDto(
                plot.Id,
                plot.PlusCode.Value,
                plot.Area.SizeInBigha,
                plot.Area.SizeInKattha,
                plot.HighwayFrontageType,
                plot.IsLeased,
                plot.OwnerId);
    }
}
