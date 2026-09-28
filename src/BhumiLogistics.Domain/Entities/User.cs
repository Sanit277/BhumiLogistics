using BhumiLogistics.Domain.Common;
using BhumiLogistics.Domain.Enums;
using BhumiLogistics.Domain.Exceptions;
using BuumiLogistics.Domain.Enums;

namespace BhumiLogistics.Domain.Entities;

public class User : BaseAuditableEntity
{
    private const int DomesticLeaseMaxDurationInYears = 35;
    private const int ForeignInvestedLeaseMaxDurationInYears = 50;
    private const int ForeignInvestedLeaseExtendedMaxDurationInYears = 75;

    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public string PasswordHash { get; private set; } = string.Empty;

    // Landowner-side tax/compliance metadata
    public OwnerType? OwnerType { get; private set; }
    public decimal? DeclaredTotalLandHoldingInKattha { get; private set; }

    // Corporate tenant-side foreign investment metadata
    public TenantCompanyType? CompanyType { get; private set; }
    public string? FittaApprovalReferenceNumber { get; private set; }
    public string? DepartmentOfIndustryApprovalReferenceNumber { get; private set; }
    public bool ForeignInvestmentExtensionApproved { get; private set; }

    private User() { } // EF Core

    public User(
        string fullName,
        string email,
        string phoneNumber,
        UserRole role,
        string passwordHash,
        OwnerType? ownerType,
        decimal? declaredTotalLandHoldingInKattha,
        TenantCompanyType? companyType,
        string? fittaApprovalReferenceNumber,
        string? departmentOfIndustryApprovalReferenceNumber,
        bool foreignInvestmentExtensionApproved)
    {
        if (role == UserRole.Landowner && ownerType is null)
            throw new DomainException("A landowner account must declare whether it is an Individual or a Company.");

        if (role == UserRole.CorporateTenant && companyType is null)
            throw new DomainException("A corporate tenant account must declare its company type.");

        if (companyType == TenantCompanyType.ForeignInvestedCompany
            && (string.IsNullOrWhiteSpace(fittaApprovalReferenceNumber)
                || string.IsNullOrWhiteSpace(departmentOfIndustryApprovalReferenceNumber)))
        {
            throw new DomainException(
                "A foreign-invested tenant must provide both a FITTA approval reference and a " +
                "Department of Industry / Investment Board Nepal approval reference.");
        }

        FullName = fullName;
        Email = email;
        PhoneNumber = phoneNumber;
        Role = role;
        PasswordHash = passwordHash;
        OwnerType = ownerType;
        DeclaredTotalLandHoldingInKattha = declaredTotalLandHoldingInKattha;
        CompanyType = companyType;
        FittaApprovalReferenceNumber = fittaApprovalReferenceNumber;
        DepartmentOfIndustryApprovalReferenceNumber = departmentOfIndustryApprovalReferenceNumber;
        ForeignInvestmentExtensionApproved = foreignInvestmentExtensionApproved;
    }

    /// <summary>
    /// The statutory lease-duration ceiling that applies when this user is the tenant
    /// on a lease offer: 35 years domestically, 50 for a FITTA-approved foreign-invested
    /// tenant, or 75 if that tenant additionally holds an approved extension.
    /// </summary>
    public int MaximumLeaseDurationInYearsAsTenant()
    {
        if (CompanyType != TenantCompanyType.ForeignInvestedCompany)
            return DomesticLeaseMaxDurationInYears;

        return ForeignInvestmentExtensionApproved
            ? ForeignInvestedLeaseExtendedMaxDurationInYears
            : ForeignInvestedLeaseMaxDurationInYears;
    }
}