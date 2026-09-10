using MediatR;

namespace BhumiLogistics.Application.Features.Admin.Commands.DeleteLandPlot;

public sealed record DeleteLandPlotCommand(Guid Id) : IRequest;