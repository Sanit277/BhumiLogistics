using MediatR;

namespace BhumiLogistics.Application.Features.LandPlots.Queries.GetLandPlotById;

public sealed record GetLandPlotByIdQuery(Guid Id) : IRequest<LandPlotDto?>;
