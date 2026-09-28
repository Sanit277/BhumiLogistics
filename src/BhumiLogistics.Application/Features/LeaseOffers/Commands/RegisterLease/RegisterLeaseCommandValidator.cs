using FluentValidation;

namespace BhumiLogistics.Application.Features.LeaseOffers.Commands.RegisterLease;

public class RegisterLeaseCommandValidator : AbstractValidator<RegisterLeaseCommand>
{
    public RegisterLeaseCommandValidator()
    {
        RuleFor(x => x.DeedReferenceNumber).NotEmpty().MaximumLength(100);
        RuleFor(x => x.RegistrationDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Registration date cannot be in the future.");
    }
}