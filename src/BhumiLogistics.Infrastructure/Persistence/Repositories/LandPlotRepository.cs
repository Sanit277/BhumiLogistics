using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Entities;
using BhumiLogistics.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Infrastructure.Persistence.Repositories;

public class LandPlotRepository : ILandPlotRepository
{
    private readonly ApplicationDbContext _context;

    public LandPlotRepository(ApplicationDbContext context) => _context = context;

    public async Task<LandPlot?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _context.LandPlots
            .Include(p => p.LeaseOffers)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task<IReadOnlyList<LandPlot>> GetAvailablePlotsByHighwayTypeAsync(
        HighwayType? highwayType, CancellationToken cancellationToken)
    {
        var query = _context.LandPlots.Where(p => !p.IsLeased);

        if (highwayType.HasValue)
            query = query.Where(p => p.HighwayFrontageType == highwayType.Value);

        return await query.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task AddAsync(LandPlot landPlot, CancellationToken cancellationToken) =>
        await _context.LandPlots.AddAsync(landPlot, cancellationToken);
}
