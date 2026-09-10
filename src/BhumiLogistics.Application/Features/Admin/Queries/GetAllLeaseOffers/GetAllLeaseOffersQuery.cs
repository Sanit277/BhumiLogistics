using BhumiLogistics.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Application.Features.Admin.Queries.GetAllLeaseOffers;

public sealed record GetAllLeaseOffersQuery : IRequest<IReadOnlyList<AdminLeaseOfferDto>>;

public class GetAllLeaseOffersQueryHandler
    : IRequestHandler<GetAllLeaseOffersQuery, IReadOnlyList<AdminLeaseOfferDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllLeaseOffersQueryHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<IReadOnlyList<AdminLeaseOfferDto>> Handle(
        GetAllLeaseOffersQuery request, CancellationToken cancellationToken) =>
        await _dbContext.LeaseOffers
            .OrderByDescending(o => o.CreatedAtUtc)
            .Join(_dbContext.LandPlots,
                offer => offer.LandPlotId,
                plot => plot.Id,
                (offer, plot) => new AdminLeaseOfferDto(
                    offer.Id,
                    offer.LandPlotId,
                    plot.PlusCode.Value,
                    offer.TenantName,
                    offer.OfferedAmount,
                    offer.DurationInYears,
                    offer.Status,
                    offer.ProposedStartDate,
                    offer.CreatedAtUtc))
            .ToListAsync(cancellationToken);
}