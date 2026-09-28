using MediatR;

namespace BhumiLogistics.Application.Features.LeaseOffers.Commands.MarkLeasePendingRegistration;

public sealed record MarkLeasePendingRegistrationCommand(Guid LeaseOfferId) : IRequest;