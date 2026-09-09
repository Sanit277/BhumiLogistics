using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Exceptions;
using MediatR;

namespace BhumiLogistics.Application.Features.LeaseOffers.Commands.AcceptLeaseOffer;

/// <summary>
/// Accepts a specific lease offer on a plot: marks that offer Accepted,
/// rejects all other pending offers on the same plot, and marks the plot leased.
/// </summary>
public class AcceptLeaseOfferCommandHandler : IRequestHandler<AcceptLeaseOfferCommand>
{
    private readonly ILandPlotRepository _landPlotRepository;
    private readonly IApplicationDbContext _dbContext;

    public AcceptLeaseOfferCommandHandler(ILandPlotRepository landPlotRepository, IApplicationDbContext dbContext)
    {
        _landPlotRepository = landPlotRepository;
        _dbContext = dbContext;
    }

    public async Task Handle(AcceptLeaseOfferCommand request, CancellationToken cancellationToken)
    {
        var landPlot = await _landPlotRepository.GetByIdAsync(request.LandPlotId, cancellationToken)
            ?? throw new DomainException($"Land plot '{request.LandPlotId}' was not found.");

        var offerToAccept = landPlot.LeaseOffers.FirstOrDefault(o => o.Id == request.LeaseOfferId)
            ?? throw new DomainException($"Lease offer '{request.LeaseOfferId}' was not found on this plot.");

        foreach (var offer in landPlot.LeaseOffers.Where(o => o.Id != offerToAccept.Id))
            offer.Reject();

        offerToAccept.Accept();
        landPlot.MarkAsLeased(); // domain guard already prevents double-leasing here

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
