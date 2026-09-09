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
    string Description) : IRequest<Guid>; // OwnerId is derived from the authenticated JWT, not the request body
