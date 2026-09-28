using BhumiLogistics.Domain.Enums;
using BuumiLogistics.Domain.Enums;
using MediatR;

namespace BhumiLogistics.Application.Features.Auth.Commands.Register;

public sealed record RegisterCommand(
    string FullName,
    string Email,
    string PhoneNumber,
    string Password,
    UserRole Role,
    OwnerType? OwnerType,
    decimal? DeclaredTotalLandHoldingInKattha,
    TenantCompanyType? CompanyType,
    string? FittaApprovalReferenceNumber,
    string? DepartmentOfIndustryApprovalReferenceNumber,
    bool ForeignInvestmentExtensionApproved) : IRequest<Guid>;