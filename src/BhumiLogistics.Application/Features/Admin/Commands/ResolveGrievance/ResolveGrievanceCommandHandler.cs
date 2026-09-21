using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Application.Features.Admin.Commands.ResolveGrievance;

public class ResolveGrievanceCommandHandler : IRequestHandler<ResolveGrievanceCommand>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public ResolveGrievanceCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task Handle(ResolveGrievanceCommand request, CancellationToken cancellationToken)
    {
        var adminId = _currentUserService.UserId
            ?? throw new DomainException("Unable to determine the authenticated admin.");

        var grievance = await _dbContext.Grievances.FirstOrDefaultAsync(g => g.Id == request.GrievanceId, cancellationToken)
            ?? throw new DomainException($"Grievance '{request.GrievanceId}' was not found.");

        grievance.Resolve(adminId, request.ResolutionNotes);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}