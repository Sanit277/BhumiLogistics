using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Application.Features.Admin.Queries.GetHighwaySetbackStandards;

public sealed record HighwaySetbackStandardDto(HighwayType HighwayFrontageType, decimal SetbackDistanceInMeters, string? Notes);

public sealed record GetHighwaySetbackStandardsQuery : IRequest<IReadOnlyList<HighwaySetbackStandardDto>>;

public class GetHighwaySetbackStandardsQueryHandler
    : IRequestHandler<GetHighwaySetbackStandardsQuery, IReadOnlyList<HighwaySetbackStandardDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetHighwaySetbackStandardsQueryHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<IReadOnlyList<HighwaySetbackStandardDto>> Handle(
        GetHighwaySetbackStandardsQuery request, CancellationToken cancellationToken) =>
        await _dbContext.HighwaySetbackStandards
            .Select(s => new HighwaySetbackStandardDto(s.HighwayFrontageType, s.SetbackDistanceInMeters, s.Notes))
            .ToListAsync(cancellationToken);
}