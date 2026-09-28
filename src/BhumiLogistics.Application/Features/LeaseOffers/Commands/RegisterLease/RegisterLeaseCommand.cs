using MediatR;

namespace BhumiLogistics.Application.Features.LeaseOffers.Commands.RegisterLease;

public sealed record RegisterLeaseCommand(Guid LeaseOfferId, string DeedReferenceNumber, DateOnly RegistrationDate) : IRequest;