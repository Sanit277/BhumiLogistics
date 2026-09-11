using MediatR;

namespace BhumiLogistics.Application.Features.Admin.Commands.RejectLandPlotOwnership;

public sealed record RejectLandPlotOwnershipCommand(Guid LandPlotId, string Reason) : IRequest;