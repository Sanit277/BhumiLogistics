using BhumiLogistics.Domain.Entities;
using BhumiLogistics.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BhumiLogistics.Infrastructure.Persistence.Configurations;

public class HighwaySetbackStandardConfiguration : IEntityTypeConfiguration<HighwaySetbackStandard>
{
    public void Configure(EntityTypeBuilder<HighwaySetbackStandard> builder)
    {
        builder.ToTable("HighwaySetbackStandards");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.HighwayFrontageType).HasConversion<string>().HasMaxLength(50);
        builder.Property(x => x.SetbackDistanceInMeters).HasPrecision(6, 2);
        builder.Property(x => x.Notes).HasMaxLength(500);

        builder.HasIndex(x => x.HighwayFrontageType).IsUnique();

        builder.Ignore(x => x.DomainEvents);

        // Seed starting values. These are approximate figures pending confirmation
        // against current Department of Roads regulation — admin-editable via the
        // /api/admin/highway-setback-standards endpoints, not meant to be final.
        builder.HasData(
            new { Id = Guid.Parse("11111111-0000-0000-0000-000000000001"), HighwayFrontageType = HighwayType.None, SetbackDistanceInMeters = 0m, Notes = (string?)null },
            new { Id = Guid.Parse("11111111-0000-0000-0000-000000000002"), HighwayFrontageType = HighwayType.StateHighwayFrontage, SetbackDistanceInMeters = 15m, Notes = "Approximate — verify against current Department of Roads standard." },
            new { Id = Guid.Parse("11111111-0000-0000-0000-000000000003"), HighwayFrontageType = HighwayType.NationalHighwayFrontage, SetbackDistanceInMeters = 25m, Notes = "Approximate — verify against current Department of Roads standard." },
            new { Id = Guid.Parse("11111111-0000-0000-0000-000000000004"), HighwayFrontageType = HighwayType.ExpresswayFrontage, SetbackDistanceInMeters = 50m, Notes = "Approximate — draft 2083 bill proposes 50m per side plus an additional 6m no-build buffer." },
            new { Id = Guid.Parse("11111111-0000-0000-0000-000000000005"), HighwayFrontageType = HighwayType.CornerPlotDualFrontage, SetbackDistanceInMeters = 25m, Notes = "Conservative default for dual frontage — confirm which of the two roads governs." }
        );
    }
}