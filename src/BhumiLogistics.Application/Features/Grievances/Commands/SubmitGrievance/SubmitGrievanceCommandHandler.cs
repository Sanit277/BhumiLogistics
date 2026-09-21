using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Entities;
using BhumiLogistics.Domain.Exceptions;
using MediatR;

namespace BhumiLogistics.Application.Features.Grievances.Commands.SubmitGrievance;

public class SubmitGrievanceCommandHandler : IRequestHandler<SubmitGrievanceCommand, Guid>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public SubmitGrievanceCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(SubmitGrievanceCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId
            ?? throw new DomainException("Unable to determine the authenticated user.");

        var grievance = Grievance.Submit(userId, request.Subject, request.Description);

        await _dbContext.Grievances.AddAsync(grievance, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return grievance.Id;
    }
}