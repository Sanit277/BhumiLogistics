using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Exceptions;
using MediatR;

namespace BhumiLogistics.Application.Features.Admin.Commands.VerifyLandPlotOwnership;

public class VerifyLandPlotOwnershipCommandHandler : IRequestHandler<VerifyLandPlotOwnershipCommand>
{
    private readonly ILandPlotRepository _landPlotRepository;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public VerifyLandPlotOwnershipCommandHandler(
        ILandPlotRepository landPlotRepository, IApplicationDbContext dbContext, ICurrentUserService currentUserService)
    {
        _landPlotRepository = landPlotRepository;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task Handle(VerifyLandPlotOwnershipCommand request, CancellationToken cancellationToken)
    {
        var adminId = _currentUserService.UserId
            ?? throw new DomainException("Unable to determine the authenticated admin.");

        var plot = await _landPlotRepository.GetByIdAsync(request.LandPlotId, cancellationToken)
            ?? throw new DomainException($"Land plot '{request.LandPlotId}' was not found.");

        plot.VerifyOwnership(adminId, request.Notes);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}