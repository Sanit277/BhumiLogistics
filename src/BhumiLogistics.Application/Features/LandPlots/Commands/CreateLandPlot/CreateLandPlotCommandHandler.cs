using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Entities;
using BhumiLogistics.Domain.Exceptions;
using BhumiLogistics.Domain.ValueObjects;
using MediatR;

namespace BhumiLogistics.Application.Features.LandPlots.Commands.CreateLandPlot;

/// <summary>
/// Handles the creation of a new highway-connected land listing.
/// Demonstrates the clean data flow: Command → Handler → Domain factory → Repository.
/// </summary>
public class CreateLandPlotCommandHandler : IRequestHandler<CreateLandPlotCommand, Guid>
{
    private readonly ILandPlotRepository _landPlotRepository;
    private readonly IApplicationDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public CreateLandPlotCommandHandler(
        ILandPlotRepository landPlotRepository,
        IApplicationDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _landPlotRepository = landPlotRepository;
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(CreateLandPlotCommand request, CancellationToken cancellationToken)
    {
        var ownerId = _currentUserService.UserId
            ?? throw new DomainException("Unable to determine the authenticated owner.");

        var plusCode = PlusCode.Create(request.PlusCode);
        var area = LandArea.Create(request.SizeInBigha, request.SizeInKattha);

        var landPlot = LandPlot.List(
            plusCode,
            area,
            request.HighwayFrontageType,
            ownerId,
            request.Latitude,
            request.Longitude,
            request.Description);

        await _landPlotRepository.AddAsync(landPlot, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return landPlot.Id;
    }
}
