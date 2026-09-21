using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Application.Features.Grievances.Queries.GetMyGrievances;

public class GetMyGrievancesQueryHandler : IRequestHandler<GetMyGrievancesQuery, IReadOnlyList<MyGrievanceDto>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public GetMyGrievancesQueryHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<IReadOnlyList<MyGrievanceDto>> Handle(GetMyGrievancesQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new DomainException("Unable to determine the authenticated user.");

        return await _dbContext.Grievances
            .Where(g => g.SubmittedByUserId == userId)
            .OrderByDescending(g => g.SubmittedAtUtc)
            .Select(g => new MyGrievanceDto(
                g.Id, g.Subject, g.Description, g.Status, g.SubmittedAtUtc, g.ExpectedResponseByUtc, g.ResolutionNotes))
            .ToListAsync(cancellationToken);
    }
}