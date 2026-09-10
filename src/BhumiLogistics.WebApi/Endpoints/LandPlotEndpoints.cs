using BhumiLogistics.Application.Features.LandPlots.Commands.CreateLandPlot;
using BhumiLogistics.Application.Features.LandPlots.Queries.GetAvailableHighwayPlots;
using BhumiLogistics.Application.Features.LandPlots.Queries.GetLandPlotById;
using BhumiLogistics.Domain.Enums;
using MediatR;

namespace BhumiLogistics.WebApi.Endpoints;

/// <summary>
/// Minimal API endpoints for land plot listing and discovery.
/// Controllers remain "thin" — all logic is delegated to MediatR handlers.
/// </summary>
public static class LandPlotEndpoints
{
    public static void MapLandPlotEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/land-plots").WithTags("Land Plots");

        group.MapPost("/", async (CreateLandPlotCommand command, ISender sender) =>
        {
            var id = await sender.Send(command);
            return Results.Created($"/api/land-plots/{id}", new { id });
        })
        .WithName("CreateLandPlot")
        .RequireAuthorization(policy => policy.RequireRole("Landowner")) 
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        group.MapGet("/{id:guid}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetLandPlotByIdQuery(id));
            return result is not null ? Results.Ok(result) : Results.NotFound();
        })
        .WithName("GetLandPlotById")
        .Produces<LandPlotDto>()
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/available", async (HighwayType? highwayFrontageType, ISender sender) =>
        {
            var result = await sender.Send(new GetAvailableHighwayPlotsQuery(highwayFrontageType));
            return Results.Ok(result);
        })
        .WithName("GetAvailableHighwayPlots")
        .Produces<IReadOnlyList<LandPlotSummaryDto>>();
    }
}
