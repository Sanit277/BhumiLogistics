using BhumiLogistics.Domain.Enums;
using MediatR;

namespace BhumiLogistics.Application.Features.Admin.Commands.UpdateHighwaySetbackStandard;

public sealed record UpdateHighwaySetbackStandardCommand(
    HighwayType HighwayFrontageType, decimal SetbackDistanceInMeters, string? Notes) : IRequest;