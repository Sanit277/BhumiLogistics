using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Enums;
using BhumiLogistics.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Application.Features.LeaseOffers.Queries.GetMyLeaseOffers;

public sealed record MyLeaseOfferDto(
    Guid Id,
    Guid LandPlotId,
    string PlotPlusCode,
    decimal OfferedAmount,
    int DurationInYears,
    OfferStatus Status,
    DateOnly ProposedStartDate,
    MalpotRegistrationStatus MalpotRegistrationStatus,
    string? RegisteredDeedReferenceNumber,
    DateTimeOffset CreatedAtUtc);

public sealed record GetMyLeaseOffersQuery : IRequest<IReadOnlyList<MyLeaseOfferDto>>;

public class GetMyLeaseOffersQueryHandler : IRequestHandler<GetMyLeaseOffersQuery, IReadOnlyList<MyLeaseOfferDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public GetMyLeaseOffersQueryHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<MyLeaseOfferDto>> Handle(GetMyLeaseOffersQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new DomainException("Unable to determine the authenticated user.");

        var query =
            from offer in _dbContext.LeaseOffers
            join plot in _dbContext.LandPlots on offer.LandPlotId equals plot.Id
            where offer.TenantUserId == userId
            orderby offer.CreatedAtUtc descending
            select new MyLeaseOfferDto(
                offer.Id,
                offer.LandPlotId,
                plot.PlusCode.Value,
                offer.OfferedAmount,
                offer.DurationInYears,
                offer.Status,
                offer.ProposedStartDate,
                offer.MalpotRegistrationStatus,
                offer.RegisteredDeedReferenceNumber,
                offer.CreatedAtUtc);

        return await query.ToListAsync(cancellationToken);
    }
}