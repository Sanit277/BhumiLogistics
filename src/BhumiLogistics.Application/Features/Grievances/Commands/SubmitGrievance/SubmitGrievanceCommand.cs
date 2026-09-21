using MediatR;

namespace BhumiLogistics.Application.Features.Grievances.Commands.SubmitGrievance;

public sealed record SubmitGrievanceCommand(string Subject, string Description) : IRequest<Guid>;