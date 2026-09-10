using BhumiLogistics.Application.Common.Interfaces;
using BhumiLogistics.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BhumiLogistics.Application.Features.Admin.Commands.DeleteLandPlot;

public class DeleteLandPlotCommandHandler : IRequestHandler<DeleteLandPlotCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteLandPlotCommandHandler(IApplicationDbContext dbContext) => _dbContext = dbContext;

    public async Task Handle(DeleteLandPlotCommand request, CancellationToken cancellationToken)
    {
        var plot = await _dbContext.LandPlots.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
            ?? throw new DomainException($"Land plot '{request.Id}' was not found.");

        _dbContext.LandPlots.Remove(plot);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}