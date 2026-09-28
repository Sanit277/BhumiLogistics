using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Exceptions;
using MediatR;

namespace BhumiLogistics.Application.Features.LeaseOffers.Commands.RegisterLease;

public class RegisterLeaseCommandHandler : IRequestHandler<RegisterLeaseCommand>
{
    private readonly ILeaseOfferRepository _leaseOfferRepository;
    private readonly ILandPlotRepository _landPlotRepository;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public RegisterLeaseCommandHandler(
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

    public async Task Handle(RegisterLeaseCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new DomainException("Unable to determine the authenticated user.");

        var offer = await _leaseOfferRepository.GetByIdAsync(request.LeaseOfferId, cancellationToken)
            ?? throw new DomainException($"Lease offer '{request.LeaseOfferId}' was not found.");

        var plot = await _landPlotRepository.GetByIdAsync(offer.LandPlotId, cancellationToken)
            ?? throw new DomainException($"Land plot '{offer.LandPlotId}' was not found.");

        if (plot.OwnerId != userId)
            throw new DomainException("Only the landowner of this plot can register the lease.");

        offer.MarkRegistered(request.DeedReferenceNumber, request.RegistrationDate, userId);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}