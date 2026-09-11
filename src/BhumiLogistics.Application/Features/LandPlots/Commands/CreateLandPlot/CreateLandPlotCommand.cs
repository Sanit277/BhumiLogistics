using BhumiLogistics.Domain.Enums;
using MediatR;

namespace BhumiLogistics.Application.Features.LandPlots.Commands.CreateLandPlot;

public sealed record CreateLandPlotCommand(
    string PlusCode,
    decimal SizeInBigha,
    decimal SizeInKattha,
    HighwayType HighwayFrontageType,
    decimal Latitude,
    decimal Longitude,
    string Description,
    string RegisteredOwnerName,
    OwnerRelationship RelationshipToOwner,
    string? PowerOfAttorneyReferenceNumber,
    string LalpurjaReferenceNumber,
    string KittaNumber,
    string WardMunicipality,
    string? LandIdentityNumber,
    LandTenureType TenureType,
    bool MohiTenancyDeclared,
    string? MohiTenancyNotes) : IRequest<Guid>;