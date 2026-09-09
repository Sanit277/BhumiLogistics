using BhumiLogistics.Domain.Entities;
using BhumiLogistics.Domain.Enums;

namespace BhumiLogistics.Application.Common.Interfaces;

public interface ILandPlotRepository
{
    Task<LandPlot?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<LandPlot>> GetAvailablePlotsByHighwayTypeAsync(
        HighwayType? highwayType,
        CancellationToken cancellationToken);

    Task AddAsync(LandPlot landPlot, CancellationToken cancellationToken);
}
