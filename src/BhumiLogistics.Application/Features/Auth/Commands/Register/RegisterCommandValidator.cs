using BhumiLogistics.Domain.Enums;
using FluentValidation;

namespace BhumiLogistics.Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.");

        RuleFor(x => x.OwnerType)
            .NotNull()
            .When(x => x.Role == UserRole.Landowner)
            .WithMessage("Landowner accounts must declare whether they are an Individual or a Company.");

        RuleFor(x => x.CompanyType)
            .NotNull()
            .When(x => x.Role == UserRole.CorporateTenant)
            .WithMessage("Corporate tenant accounts must declare their company type.");

        RuleFor(x => x.FittaApprovalReferenceNumber)
            .NotEmpty()
            .When(x => x.CompanyType == TenantCompanyType.ForeignInvestedCompany)
            .WithMessage("A FITTA approval reference is required for a foreign-invested tenant.");

        RuleFor(x => x.DepartmentOfIndustryApprovalReferenceNumber)
            .NotEmpty()
            .When(x => x.CompanyType == TenantCompanyType.ForeignInvestedCompany)
            .WithMessage("A Department of Industry / Investment Board Nepal approval reference is required for a foreign-invested tenant.");
    }
}