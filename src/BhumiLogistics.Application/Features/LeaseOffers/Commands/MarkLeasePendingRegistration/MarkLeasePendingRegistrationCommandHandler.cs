using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Exceptions;
using MediatR;

namespace BhumiLogistics.Application.Features.LeaseOffers.Commands.MarkLeasePendingRegistration;

/// <summary>
/// Restricted to the landowner of the plot the offer belongs to — the same
/// ownership check pattern that AcceptLeaseOfferCommandHandler is still missing
/// and should eventually receive too.
/// </summary>
public class MarkLeasePendingRegistrationCommandHandler : IRequestHandler<MarkLeasePendingRegistrationCommand>
{
    private readonly ILeaseOfferRepository _leaseOfferRepository;
    private readonly ILandPlotRepository _landPlotRepository;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public MarkLeasePendingRegistrationCommandHandler(
        ILeaseOfferRepository leaseOfferRepository,
        ILandPlotRepository landPlotRepository,
        IApplicationDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _leaseOfferRepository = leaseOfferRepository;
        _landPlotRepository = landPlotRepository;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task Handle(MarkLeasePendingRegistrationCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new DomainException("Unable to determine the authenticated user.");

        var offer = await _leaseOfferRepository.GetByIdAsync(request.LeaseOfferId, cancellationToken)
            ?? throw new DomainException($"Lease offer '{request.LeaseOfferId}' was not found.");

        var plot = await _landPlotRepository.GetByIdAsync(offer.LandPlotId, cancellationToken)
            ?? throw new DomainException($"Land plot '{offer.LandPlotId}' was not found.");

        if (plot.OwnerId != userId)
            throw new DomainException("Only the landowner of this plot can update the lease's registration status.");

        offer.MarkPendingRegistration();
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}