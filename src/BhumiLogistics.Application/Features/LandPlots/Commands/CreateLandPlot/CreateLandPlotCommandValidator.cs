using FluentValidation;

namespace BhumiLogistics.Application.Features.LandPlots.Commands.CreateLandPlot;

public class CreateLandPlotCommandValidator : AbstractValidator<CreateLandPlotCommand>
{
    public CreateLandPlotCommandValidator()
    {
        RuleFor(x => x.PlusCode)
            .NotEmpty().WithMessage("Plus Code is required to geo-tag the plot.");

        RuleFor(x => x)
            .Must(x => x.SizeInBigha > 0 || x.SizeInKattha > 0)
            .WithMessage("Land area must be specified in at least Bigha or Kattha.");

        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
    }
}
