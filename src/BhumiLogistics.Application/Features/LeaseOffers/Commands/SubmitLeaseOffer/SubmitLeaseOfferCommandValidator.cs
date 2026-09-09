using FluentValidation;

namespace BhumiLogistics.Application.Features.LeaseOffers.Commands.SubmitLeaseOffer;

public class SubmitLeaseOfferCommandValidator : AbstractValidator<SubmitLeaseOfferCommand>
{
    public SubmitLeaseOfferCommandValidator()
    {
        RuleFor(x => x.LandPlotId).NotEmpty();
        RuleFor(x => x.TenantName).NotEmpty().MaximumLength(200);
        RuleFor(x => x.OfferedAmount).GreaterThan(0);
        RuleFor(x => x.DurationInYears).InclusiveBetween(1, 99);
        RuleFor(x => x.ProposedStartDate)
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Proposed start date cannot be in the past.");
    }
}
