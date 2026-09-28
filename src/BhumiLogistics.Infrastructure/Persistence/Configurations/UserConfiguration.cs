using BhumiLogistics.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BhumiLogistics.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FullName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Email).HasMaxLength(256).IsRequired();
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.Role).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.PasswordHash).IsRequired();

        builder.Property(x => x.OwnerType).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.DeclaredTotalLandHoldingInKattha).HasPrecision(12, 2);
        builder.Property(x => x.CompanyType).HasConversion<string>().HasMaxLength(30);
        builder.Property(x => x.FittaApprovalReferenceNumber).HasMaxLength(100);
        builder.Property(x => x.DepartmentOfIndustryApprovalReferenceNumber).HasMaxLength(100);

        builder.Ignore(x => x.DomainEvents);
    }
}
