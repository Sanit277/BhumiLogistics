using BhumiLogistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BhumiLogistics.Infrastructure.Persistence.Configurations;

public class PlatformSettingsConfiguration : IEntityTypeConfiguration<PlatformSettings>
{
    public void Configure(EntityTypeBuilder<PlatformSettings> builder)
    {
        builder.ToTable("PlatformSettings");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.BusinessName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.PanNumber).HasMaxLength(30).IsRequired();
        builder.Property(x => x.VatNumber).HasMaxLength(30);
        builder.Property(x => x.RegisteredAddress).HasMaxLength(500).IsRequired();
        builder.Property(x => x.ContactEmail).HasMaxLength(256).IsRequired();
        builder.Property(x => x.ContactPhone).HasMaxLength(30).IsRequired();
        builder.Property(x => x.GrievanceOfficerName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.GrievanceOfficerEmail).HasMaxLength(256).IsRequired();
        builder.Property(x => x.GrievanceOfficerPhone).HasMaxLength(30).IsRequired();

        builder.Ignore(x => x.DomainEvents);

        // Seed exactly one row with placeholder values — an admin MUST update these
        // via PUT /api/admin/platform-settings before real launch. Ids here use
        // BaseAuditableEntity's default Guid.NewGuid(), so we pin a fixed Id for HasData.
        builder.HasData(new
{
            Id = Guid.Parse("22222222-0000-0000-0000-000000000001"),
            BusinessName = "BhumiLogistics Pvt. Ltd. (PLACEHOLDER — update before launch)",
            PanNumber = "000000000",
            VatNumber = (string?)null,
            RegisteredAddress = "PLACEHOLDER ADDRESS — update before launch",
            ContactEmail = "contact@bhumilogistics.example",
            ContactPhone = "0000000000",
            GrievanceOfficerName = "PLACEHOLDER — assign a real grievance officer",
            GrievanceOfficerEmail = "grievance@bhumilogistics.example",
            GrievanceOfficerPhone = "0000000000",
            CreatedAtUtc = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
        });
    }
}