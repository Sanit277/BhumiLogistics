using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<LandPlot> LandPlots => Set<LandPlot>();
    public DbSet<LeaseOffer> LeaseOffers => Set<LeaseOffer>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
