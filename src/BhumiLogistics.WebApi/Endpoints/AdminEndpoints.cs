using BhumiLogistics.Application.Features.Admin.Commands.DeleteLandPlot;
using BhumiLogistics.Application.Features.Admin.Queries.GetAllLandPlots;
using BhumiLogistics.Application.Features.Admin.Queries.GetAllLeaseOffers;
using BhumiLogistics.Application.Features.Admin.Queries.GetAllUsers;
using BhumiLogistics.Application.Features.Admin.Commands.VerifyLandPlotOwnership;
using BhumiLogistics.Application.Features.Admin.Commands.RejectLandPlotOwnership;
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

        group.MapPut("/land-plots/{id:guid}/verify-ownership", async (
            Guid id, VerifyOwnershipRequest body, ISender sender) =>
        {
            await sender.Send(new VerifyLandPlotOwnershipCommand(id, body.Notes));
            return Results.NoContent();
        })
        .WithName("AdminVerifyLandPlotOwnership")
        .Produces(StatusCodes.Status204NoContent);

        group.MapPut("/land-plots/{id:guid}/reject-ownership", async (
            Guid id, RejectOwnershipRequest body, ISender sender) =>
        {
            await sender.Send(new RejectLandPlotOwnershipCommand(id, body.Reason));
            return Results.NoContent();
        })
        .WithName("AdminRejectLandPlotOwnership")
        .Produces(StatusCodes.Status204NoContent);
    }
}
public sealed record VerifyOwnershipRequest(string? Notes);
public sealed record RejectOwnershipRequest(string Reason);