using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Entities;
using BhumiLogistics.Domain.Exceptions;
using BhumiLogistics.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace BhumiLogistics.Application.Features.LandPlots.Commands.CreateLandPlot;

public class CreateLandPlotCommandHandler : IRequestHandler<CreateLandPlotCommand, Guid>
{
    private readonly ILandPlotRepository _landPlotRepository;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public CreateLandPlotCommandHandler(
        ILandPlotRepository landPlotRepository,
        IApplicationDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _landPlotRepository = landPlotRepository;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateLandPlotCommand request, CancellationToken cancellationToken)
    {
        var ownerId = _currentUserService.UserId
            ?? throw new DomainException("Unable to determine the authenticated owner.");

        var plusCode = PlusCode.Create(request.PlusCode);
        var area = LandArea.Create(request.SizeInBigha, request.SizeInKattha);

        var ownershipVerification = OwnershipVerification.Declare(
            request.RegisteredOwnerName,
            request.RelationshipToOwner,
            request.PowerOfAttorneyReferenceNumber,
            request.LalpurjaReferenceNumber,
            request.KittaNumber,
            request.WardMunicipality,
            request.LandIdentityNumber,
            request.TenureType,
            request.MohiTenancyDeclared,
            request.MohiTenancyNotes);

        var landUseDeclaration = LandUseDeclaration.Create(
            request.LandUseClassification,
            request.LandUseConversionApprovalReferenceNumber,
            request.LandUseConversionApprovingAuthority,
            request.LandUseConversionApprovalDate);

        // Snapshot the current admin-configured setback standard for this highway type.
        var setbackStandard = await _dbContext.HighwaySetbackStandards
            .FirstOrDefaultAsync(s => s.HighwayFrontageType == request.HighwayFrontageType, cancellationToken);
        var setbackDistance = setbackStandard?.SetbackDistanceInMeters ?? 0;

        var landPlot = LandPlot.List(
            plusCode,
            area,
            request.HighwayFrontageType,
            ownerId,
            request.Latitude,
            request.Longitude,
            request.Description,
            ownershipVerification,
            landUseDeclaration,
            request.FrontageLengthInMeters,
            setbackDistance);

        await _landPlotRepository.AddAsync(landPlot, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return landPlot.Id;
    }
}