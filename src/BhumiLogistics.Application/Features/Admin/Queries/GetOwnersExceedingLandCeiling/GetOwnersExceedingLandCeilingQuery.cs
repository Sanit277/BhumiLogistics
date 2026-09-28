using BhumiLogistics.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Application.Features.Admin.Queries.GetOwnersExceedingLandCeiling;

public sealed record OwnerLandCeilingFlagDto(
    Guid OwnerId,
    string OwnerFullName,
    string OwnerEmail,
    decimal? DeclaredTotalLandHoldingInKattha,
    decimal ActualVerifiedPlotAreaInKattha,
    decimal ReviewThresholdInKattha);

public sealed record GetOwnersExceedingLandCeilingQuery : IRequest<IReadOnlyList<OwnerLandCeilingFlagDto>>;

/// <summary>
/// Flags any landowner whose self-declared holding, OR whose combined verified
/// plot area actually listed on this platform, exceeds the admin-configured
/// review threshold. This is a review trigger, not an automatic block — real
/// land-ceiling determinations require checking holdings outside this platform too.
/// </summary>
public class GetOwnersExceedingLandCeilingQueryHandler
    : IRequestHandler<GetOwnersExceedingLandCeilingQuery, IReadOnlyList<OwnerLandCeilingFlagDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetOwnersExceedingLandCeilingQueryHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<IReadOnlyList<OwnerLandCeilingFlagDto>> Handle(
        GetOwnersExceedingLandCeilingQuery request, CancellationToken cancellationToken)
    {
        var threshold = (await _dbContext.PlatformSettings.FirstAsync(cancellationToken)).LandCeilingReviewThresholdInKattha;

        var ownerAreaTotals = await _dbContext.LandPlots
            .Where(p => p.OwnershipVerification.Status == Domain.Enums.OwnershipVerificationStatus.Verified)
            .GroupBy(p => p.OwnerId)
            .Select(g => new
            {
                OwnerId = g.Key,
                TotalAreaInKattha = g.Sum(p => p.Area.SizeInBigha * 20 + p.Area.SizeInKattha)
            })
            .ToListAsync(cancellationToken);

        var owners = await _dbContext.Users.ToListAsync(cancellationToken);

        var flagged = new List<OwnerLandCeilingFlagDto>();

        foreach (var owner in owners)
        {
            var actualArea = ownerAreaTotals.FirstOrDefault(o => o.OwnerId == owner.Id)?.TotalAreaInKattha ?? 0;
            var declared = owner.DeclaredTotalLandHoldingInKattha;

            if (actualArea > threshold || declared > threshold)
            {
                flagged.Add(new OwnerLandCeilingFlagDto(
                    owner.Id, owner.FullName, owner.Email, declared, actualArea, threshold));
            }
        }

        return flagged;
    }
}