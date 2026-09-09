using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Entities;
using BhumiLogistics.Domain.Exceptions;
using MediatR;

namespace BhumiLogistics.Application.Features.LeaseOffers.Commands.SubmitLeaseOffer;

/// <summary>
/// Handles submission of a corporate tenant's lease offer against an
/// existing land plot. Notification to the landowner is handled entirely
/// by the LeaseOfferSubmittedEvent handler — this handler only creates the offer.
/// </summary>
public class SubmitLeaseOfferCommandHandler : IRequestHandler<SubmitLeaseOfferCommand, Guid>
{
    private readonly ILandPlotRepository _landPlotRepository;
    private readonly ILeaseOfferRepository _leaseOfferRepository;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public SubmitLeaseOfferCommandHandler(
        ILandPlotRepository landPlotRepository,
        ILeaseOfferRepository leaseOfferRepository,
        IApplicationDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _landPlotRepository = landPlotRepository;
        _leaseOfferRepository = leaseOfferRepository;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(SubmitLeaseOfferCommand request, CancellationToken cancellationToken)
    {
        var tenantUserId = _currentUserService.UserId
            ?? throw new DomainException("Unable to determine the authenticated tenant.");

        var landPlot = await _landPlotRepository.GetByIdAsync(request.LandPlotId, cancellationToken)
            ?? throw new DomainException($"Land plot '{request.LandPlotId}' was not found.");

        var offer = LeaseOffer.Create(
            landPlot.Id,
            request.TenantName,
            tenantUserId,
            request.OfferedAmount,
            request.DurationInYears,
            request.ProposedStartDate);

        landPlot.SubmitOffer(offer); // domain guards against leased plots

        await _leaseOfferRepository.AddAsync(offer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken); // domain events dispatched here automatically

        return offer.Id;
    }
}
