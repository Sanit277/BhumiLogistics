using FluentValidation;

namespace BhumiLogistics.Application.Features.Grievances.Commands.SubmitGrievance;

public class SubmitGrievanceCommandValidator : AbstractValidator<SubmitGrievanceCommand>
{
    public SubmitGrievanceCommandValidator()
    {
        RuleFor(x => x.Subject).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(4000);
    }
}