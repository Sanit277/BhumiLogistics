using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Entities;
using BhumiLogistics.Domain.Exceptions;
using MediatR;

namespace BhumiLogistics.Application.Features.LeaseOffers.Commands.SubmitLeaseOffer;

public class SubmitLeaseOfferCommandHandler : IRequestHandler<SubmitLeaseOfferCommand, Guid>
{
    private readonly ILandPlotRepository _landPlotRepository;
    private readonly ILeaseOfferRepository _leaseOfferRepository;
    private readonly IUserRepository _userRepository;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public SubmitLeaseOfferCommandHandler(
        ILandPlotRepository landPlotRepository,
        ILeaseOfferRepository leaseOfferRepository,
        IUserRepository userRepository,
        IApplicationDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _landPlotRepository = landPlotRepository;
        _leaseOfferRepository = leaseOfferRepository;
        _userRepository = userRepository;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(SubmitLeaseOfferCommand request, CancellationToken cancellationToken)
    {
        var tenantUserId = _currentUserService.UserId
            ?? throw new DomainException("Unable to determine the authenticated tenant.");

        var tenantUser = await _userRepository.GetByIdAsync(tenantUserId, cancellationToken)
            ?? throw new DomainException("Authenticated tenant account could not be found.");

        var landPlot = await _landPlotRepository.GetByIdAsync(request.LandPlotId, cancellationToken)
            ?? throw new DomainException($"Land plot '{request.LandPlotId}' was not found.");

        var maximumDuration = tenantUser.MaximumLeaseDurationInYearsAsTenant();

        var offer = LeaseOffer.Create(
            landPlot.Id,
            request.TenantName,
            tenantUserId,
            request.OfferedAmount,
            request.DurationInYears,
            request.ProposedStartDate,
            maximumDuration);

        landPlot.SubmitOffer(offer);

        await _leaseOfferRepository.AddAsync(offer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return offer.Id;
    }
}