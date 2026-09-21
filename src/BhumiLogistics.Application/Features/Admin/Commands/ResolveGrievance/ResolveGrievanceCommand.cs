using MediatR;

namespace BhumiLogistics.Application.Features.Admin.Commands.ResolveGrievance;

public sealed record ResolveGrievanceCommand(Guid GrievanceId, string ResolutionNotes) : IRequest;