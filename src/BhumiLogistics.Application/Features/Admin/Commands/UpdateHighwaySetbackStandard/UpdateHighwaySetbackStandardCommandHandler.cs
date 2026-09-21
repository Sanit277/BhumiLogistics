using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Application.Features.Admin.Commands.UpdateHighwaySetbackStandard;

public class UpdateHighwaySetbackStandardCommandHandler : IRequestHandler<UpdateHighwaySetbackStandardCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public UpdateHighwaySetbackStandardCommandHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task Handle(UpdateHighwaySetbackStandardCommand request, CancellationToken cancellationToken)
    {
        var standard = await _dbContext.HighwaySetbackStandards
            .FirstOrDefaultAsync(s => s.HighwayFrontageType == request.HighwayFrontageType, cancellationToken)
            ?? throw new DomainException($"No setback standard exists yet for '{request.HighwayFrontageType}'.");

        standard.UpdateSetbackDistance(request.SetbackDistanceInMeters, request.Notes);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}