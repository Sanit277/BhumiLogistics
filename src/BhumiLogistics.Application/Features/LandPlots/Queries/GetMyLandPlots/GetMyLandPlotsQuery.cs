using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Enums;
using BhumiLogistics.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Application.Features.LandPlots.Queries.GetMyLandPlots;

public sealed record MyLandPlotDto(
    Guid Id,
    string PlusCode,
    decimal TotalAreaInKattha,
    decimal BuildableAreaInKattha,
    HighwayType HighwayFrontageType,
    bool IsLeased,
    int OfferCount,
    OwnershipVerificationStatus OwnershipStatus,
    string? OwnershipVerificationNotes,
    LandUseClassification LandUseClassification,
    DateTimeOffset CreatedAtUtc);

public sealed record GetMyLandPlotsQuery : IRequest<IReadOnlyList<MyLandPlotDto>>;

/// <summary>
/// Unlike GetAvailableHighwayPlotsQuery (public, verified-only) this returns
/// every plot the current user owns regardless of verification status \u2014
/// so a landowner can see a plot still pending review, or why one was rejected.
/// </summary>
public class GetMyLandPlotsQueryHandler : IRequestHandler<GetMyLandPlotsQuery, IReadOnlyList<MyLandPlotDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public GetMyLandPlotsQueryHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<MyLandPlotDto>> Handle(GetMyLandPlotsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new DomainException("Unable to determine the authenticated user.");

        return await _dbContext.LandPlots
            .Where(p => p.OwnerId == userId)
            .OrderByDescending(p => p.CreatedAtUtc)
            .Select(p => new MyLandPlotDto(
                p.Id,
                p.PlusCode.Value,
                p.Area.SizeInBigha * 20 + p.Area.SizeInKattha,
                p.BuildableAreaInKattha,
                p.HighwayFrontageType,
                p.IsLeased,
                p.LeaseOffers.Count,
                p.OwnershipVerification.Status,
                p.OwnershipVerification.VerificationNotes,
                p.LandUseDeclaration.Classification,
                p.CreatedAtUtc))
            .ToListAsync(cancellationToken);
    }
}