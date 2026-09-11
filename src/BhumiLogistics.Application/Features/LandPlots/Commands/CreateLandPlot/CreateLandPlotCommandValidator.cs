using BhumiLogistics.Domain.Enums;
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

        RuleFor(x => x.RegisteredOwnerName)
            .NotEmpty().WithMessage("The registered owner's name, exactly as shown on the Lalpurja, is required.");

        RuleFor(x => x.LalpurjaReferenceNumber)
            .NotEmpty().WithMessage("A Lalpurja reference number is required.");

        RuleFor(x => x.KittaNumber)
            .NotEmpty().WithMessage("The Kitta (plot) number is required.");

        RuleFor(x => x.WardMunicipality)
            .NotEmpty().WithMessage("The ward/municipality where the plot is located is required.");

        RuleFor(x => x.PowerOfAttorneyReferenceNumber)
            .NotEmpty()
            .When(x => x.RelationshipToOwner == OwnerRelationship.AuthorizedAgent)
            .WithMessage("A Power of Attorney reference is required when you are not the registered owner.");

        RuleFor(x => x.TenureType)
            .Must(t => t != LandTenureType.GuthiOther && t != LandTenureType.Government)
            .WithMessage("Land of this tenure type cannot be listed on this platform.");
    }
}