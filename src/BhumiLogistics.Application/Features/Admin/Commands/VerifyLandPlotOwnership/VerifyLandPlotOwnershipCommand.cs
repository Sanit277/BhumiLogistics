using MediatR;

namespace BhumiLogistics.Application.Features.Admin.Commands.VerifyLandPlotOwnership;

public sealed record VerifyLandPlotOwnershipCommand(Guid LandPlotId, string? Notes) : IRequest;