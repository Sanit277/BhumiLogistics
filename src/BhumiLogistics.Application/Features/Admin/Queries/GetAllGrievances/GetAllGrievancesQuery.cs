using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Application.Features.Admin.Queries.GetAllGrievances;

public sealed record AdminGrievanceDto(
    Guid Id, Guid SubmittedByUserId, string Subject, string Description, GrievanceStatus Status,
    DateTimeOffset SubmittedAtUtc, DateTimeOffset ExpectedResponseByUtc, bool IsOverdue, string? ResolutionNotes);

public sealed record GetAllGrievancesQuery : IRequest<IReadOnlyList<AdminGrievanceDto>>;

public class GetAllGrievancesQueryHandler : IRequestHandler<GetAllGrievancesQuery, IReadOnlyList<AdminGrievanceDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetAllGrievancesQueryHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task<IReadOnlyList<AdminGrievanceDto>> Handle(GetAllGrievancesQuery request, CancellationToken cancellationToken) =>
        await _dbContext.Grievances
            .OrderByDescending(g => g.SubmittedAtUtc)
            .Select(g => new AdminGrievanceDto(
                g.Id, g.SubmittedByUserId, g.Subject, g.Description, g.Status,
                g.SubmittedAtUtc, g.ExpectedResponseByUtc, g.IsOverdue, g.ResolutionNotes))
            .ToListAsync(cancellationToken);
}