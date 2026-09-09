using BhumiLogistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BhumiLogistics.Infrastructure.Persistence.Configurations;

public class LeaseOfferConfiguration : IEntityTypeConfiguration<LeaseOffer>
{
    public void Configure(EntityTypeBuilder<LeaseOffer> builder)
    {
        builder.ToTable("LeaseOffers");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TenantName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.OfferedAmount).HasPrecision(18, 2);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.ProposedStartDate).HasColumnType("date");

        builder.Ignore(x => x.DomainEvents);
    }
}
