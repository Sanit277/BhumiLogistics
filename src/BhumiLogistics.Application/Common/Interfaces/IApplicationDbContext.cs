using BhumiLogistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Application.Common.Interfaces;

/// <summary>
/// Abstraction over the persistence context. The Application layer depends
/// on this interface only — never on EF Core directly — preserving the
/// Dependency Inversion Principle at the architecture boundary.
/// </summary>
public interface IApplicationDbContext
{
    DbSet<LandPlot> LandPlots { get; }
    DbSet<LeaseOffer> LeaseOffers { get; }
    DbSet<User> Users { get; }
    DbSet<HighwaySetbackStandard> HighwaySetbackStandards { get; }
    DbSet<Grievance> Grievances { get; }
    DbSet<PlatformSettings> PlatformSettings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
