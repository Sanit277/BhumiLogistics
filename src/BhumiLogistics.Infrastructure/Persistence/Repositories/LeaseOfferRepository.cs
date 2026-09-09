using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Infrastructure.Persistence.Repositories;

public class LeaseOfferRepository : ILeaseOfferRepository
{
    private readonly ApplicationDbContext _context;

    public LeaseOfferRepository(ApplicationDbContext context) => _context = context;

    public async Task<LeaseOffer?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _context.LeaseOffers.FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    public async Task AddAsync(LeaseOffer leaseOffer, CancellationToken cancellationToken) =>
        await _context.LeaseOffers.AddAsync(leaseOffer, cancellationToken);
}
