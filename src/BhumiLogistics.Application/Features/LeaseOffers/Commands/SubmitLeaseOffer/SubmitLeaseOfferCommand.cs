using MediatR;

namespace BhumiLogistics.Application.Features.LeaseOffers.Commands.SubmitLeaseOffer;

public sealed record SubmitLeaseOfferCommand(
    Guid LandPlotId,
    string TenantName,
    decimal OfferedAmount,
    int DurationInYears,
    DateOnly ProposedStartDate) : IRequest<Guid>; // TenantUserId is derived from the authenticated JWT
