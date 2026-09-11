using BhumiLogistics.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Application.Features.Admin.Queries.GetAllLandPlots;

public sealed record GetAllLandPlotsQuery : IRequest<IReadOnlyList<AdminLandPlotDto>>;

public class GetAllLandPlotsQueryHandler : IRequestHandler<GetAllLandPlotsQuery, IReadOnlyList<AdminLandPlotDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllLandPlotsQueryHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<IReadOnlyList<AdminLandPlotDto>> Handle(
        GetAllLandPlotsQuery request, CancellationToken cancellationToken) =>
        await _dbContext.LandPlots
            .OrderByDescending(p => p.CreatedAtUtc)
            .Select(p => new AdminLandPlotDto(
                p.Id,
                p.PlusCode.Value,
                p.Area.SizeInBigha * 20 + p.Area.SizeInKattha,
                p.HighwayFrontageType,
                p.IsLeased,
                p.OwnerId,
                p.LeaseOffers.Count,
                p.OwnershipVerification.Status,
                p.OwnershipVerification.RegisteredOwnerName,
                p.OwnershipVerification.TenureType,
                p.OwnershipVerification.MohiTenancyDeclared,
                p.CreatedAtUtc))
            .ToListAsync(cancellationToken);
}