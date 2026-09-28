using BhumiLogistics.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Application.Features.Admin.Queries.GetAllLeaseOffers;

public sealed record GetAllLeaseOffersQuery : IRequest<IReadOnlyList<AdminLeaseOfferDto>>;

public class GetAllLeaseOffersQueryHandler
    : IRequestHandler<GetAllLeaseOffersQuery, IReadOnlyList<AdminLeaseOfferDto>>
{
    private const decimal CompanyLandlordTdsRate = 0.10m;

    private readonly IApplicationDbContext _dbContext;

    public GetAllLeaseOffersQueryHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<IReadOnlyList<AdminLeaseOfferDto>> Handle(
        GetAllLeaseOffersQuery request, CancellationToken cancellationToken)
    {
        var offersWithPlots =
            from offer in _dbContext.LeaseOffers
            join plot in _dbContext.LandPlots on offer.LandPlotId equals plot.Id
            join owner in _dbContext.Users on plot.OwnerId equals owner.Id
            orderby offer.CreatedAtUtc descending
            select new
            {
                offer.Id,
                offer.LandPlotId,
                PlotPlusCode = plot.PlusCode.Value,
                offer.TenantName,
                offer.OfferedAmount,
                offer.DurationInYears,
                offer.Status,
                offer.ProposedStartDate,
                offer.MalpotRegistrationStatus,
                offer.RegisteredDeedReferenceNumber,
                offer.RegistrationDate,
                owner.OwnerType,
                offer.CreatedAtUtc
            };

        var results = await offersWithPlots.ToListAsync(cancellationToken);

        return results
            .Select(r => new AdminLeaseOfferDto(
                r.Id, r.LandPlotId, r.PlotPlusCode, r.TenantName, r.OfferedAmount, r.DurationInYears,
                r.Status, r.ProposedStartDate, r.MalpotRegistrationStatus, r.RegisteredDeedReferenceNumber,
                r.RegistrationDate, r.OwnerType,
                r.OwnerType == Domain.Enums.OwnerType.Company ? r.OfferedAmount * CompanyLandlordTdsRate : 0,
                r.CreatedAtUtc))
            .ToList();
    }
}
