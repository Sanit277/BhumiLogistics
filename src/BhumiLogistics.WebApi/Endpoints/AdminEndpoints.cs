using BhumiLogistics.Application.Features.Admin.Commands.DeleteLandPlot;
using BhumiLogistics.Application.Features.Admin.Queries.GetAllLandPlots;
using BhumiLogistics.Application.Features.Admin.Queries.GetAllLeaseOffers;
using BhumiLogistics.Application.Features.Admin.Queries.GetAllUsers;
using MediatR;

namespace BhumiLogistics.WebApi.Endpoints;

public static class AdminEndpoints
{
    public static void MapAdminEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/admin")
            .WithTags("Admin")
            .RequireAuthorization(policy => policy.RequireRole("Administrator"));

        group.MapGet("/users", async (ISender sender) =>
            Results.Ok(await sender.Send(new GetAllUsersQuery())))
            .WithName("AdminGetAllUsers");

        group.MapGet("/land-plots", async (ISender sender) =>
            Results.Ok(await sender.Send(new GetAllLandPlotsQuery())))
            .WithName("AdminGetAllLandPlots");

        group.MapGet("/lease-offers", async (ISender sender) =>
            Results.Ok(await sender.Send(new GetAllLeaseOffersQuery())))
            .WithName("AdminGetAllLeaseOffers");

        group.MapDelete("/land-plots/{id:guid}", async (Guid id, ISender sender) =>
        {
            await sender.Send(new DeleteLandPlotCommand(id));
            return Results.NoContent();
        })
        .WithName("AdminDeleteLandPlot");
    }
}