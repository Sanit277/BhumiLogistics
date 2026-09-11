using BhumiLogistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BhumiLogistics.Infrastructure.Persistence.Configurations;

public class LandPlotConfiguration : IEntityTypeConfiguration<LandPlot>
{
    public void Configure(EntityTypeBuilder<LandPlot> builder)
    {
        builder.ToTable("LandPlots");
        builder.HasKey(x => x.Id);

        // Value Object mapping: PlusCode → owned type stored as a scalar column
        builder.OwnsOne(x => x.PlusCode, pc =>
        {
            pc.Property(p => p.Value)
              .HasColumnName("PlusCode")
              .HasMaxLength(20)
              .IsRequired();
        });

        // Value Object mapping: LandArea → two decimal columns
        builder.OwnsOne(x => x.Area, area =>
        {
            area.Property(a => a.SizeInBigha).HasColumnName("SizeInBigha").HasPrecision(10, 2);
            area.Property(a => a.SizeInKattha).HasColumnName("SizeInKattha").HasPrecision(10, 2);
        });

        builder.OwnsOne(x => x.OwnershipVerification, ov =>
        {
            ov.Property(o => o.Status).HasColumnName("OwnershipStatus").HasConversion<string>().HasMaxLength(30);
            ov.Property(o => o.RegisteredOwnerName).HasColumnName("RegisteredOwnerName").HasMaxLength(200);
            ov.Property(o => o.RelationshipToOwner).HasColumnName("RelationshipToOwner").HasConversion<string>().HasMaxLength(30);
            ov.Property(o => o.PowerOfAttorneyReferenceNumber).HasColumnName("PowerOfAttorneyReferenceNumber").HasMaxLength(100);
            ov.Property(o => o.LalpurjaReferenceNumber).HasColumnName("LalpurjaReferenceNumber").HasMaxLength(100);
            ov.Property(o => o.KittaNumber).HasColumnName("KittaNumber").HasMaxLength(50);
            ov.Property(o => o.WardMunicipality).HasColumnName("WardMunicipality").HasMaxLength(200);
            ov.Property(o => o.LandIdentityNumber).HasColumnName("LandIdentityNumber").HasMaxLength(20);
            ov.Property(o => o.TenureType).HasColumnName("TenureType").HasConversion<string>().HasMaxLength(30);
            ov.Property(o => o.MohiTenancyDeclared).HasColumnName("MohiTenancyDeclared");
            ov.Property(o => o.MohiTenancyNotes).HasColumnName("MohiTenancyNotes").HasMaxLength(1000);
            ov.Property(o => o.VerifiedByUserId).HasColumnName("OwnershipVerifiedByUserId");
            ov.Property(o => o.VerifiedAtUtc).HasColumnName("OwnershipVerifiedAtUtc");
            ov.Property(o => o.VerificationNotes).HasColumnName("OwnershipVerificationNotes").HasMaxLength(1000);
        });

        builder.Property(x => x.HighwayFrontageType).HasConversion<string>().HasMaxLength(50);
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.LatitudeCoordinate).HasPrecision(9, 6);
        builder.Property(x => x.LongitudeCoordinate).HasPrecision(9, 6);

        builder.HasMany(x => x.LeaseOffers)
               .WithOne()
               .HasForeignKey(x => x.LandPlotId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Ignore(x => x.DomainEvents);
    }
}
