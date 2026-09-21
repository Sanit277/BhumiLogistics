using BhumiLogistics.Domain.Enums;
using MediatR;

namespace BhumiLogistics.Application.Features.Grievances.Queries.GetMyGrievances;

public sealed record MyGrievanceDto(
    Guid Id, string Subject, string Description, GrievanceStatus Status,
    DateTimeOffset SubmittedAtUtc, DateTimeOffset ExpectedResponseByUtc, string? ResolutionNotes);

public sealed record GetMyGrievancesQuery : IRequest<IReadOnlyList<MyGrievanceDto>>;