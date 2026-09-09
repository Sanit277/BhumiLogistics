using MediatR;

namespace BhumiLogistics.Application.Features.LeaseOffers.Commands.AcceptLeaseOffer;

public sealed record AcceptLeaseOfferCommand(Guid LandPlotId, Guid LeaseOfferId) : IRequest;
