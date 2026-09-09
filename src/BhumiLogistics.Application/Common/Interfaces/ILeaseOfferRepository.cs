using BhumiLogistics.Domain.Entities;

namespace BhumiLogistics.Application.Common.Interfaces;

public interface ILeaseOfferRepository
{
    Task<LeaseOffer?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(LeaseOffer leaseOffer, CancellationToken cancellationToken);
}
