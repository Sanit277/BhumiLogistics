using BhumiLogistics.Application.Features.Admin.Commands.DeleteLandPlot;
using BhumiLogistics.Application.Features.Admin.Queries.GetAllLandPlots;
using BhumiLogistics.Application.Features.Admin.Queries.GetAllLeaseOffers;
using BhumiLogistics.Application.Features.Admin.Queries.GetAllUsers;
using BhumiLogistics.Application.Features.Admin.Commands.VerifyLandPlotOwnership;
using BhumiLogistics.Application.Features.Admin.Commands.RejectLandPlotOwnership;
using BhumiLogistics.Application.Features.Admin.Queries.GetHighwaySetbackStandards;
using BhumiLogistics.Application.Features.Admin.Commands.UpdateHighwaySetbackStandard;
using BhumiLogistics.Application.Features.Admin.Queries.GetAllGrievances;
using BhumiLogistics.Application.Features.Admin.Commands.ResolveGrievance;
using BhumiLogistics.Application.Features.Admin.Commands.UpdatePlatformSettings;
using BhumiLogistics.Application.Features.Platform.Queries.GetPlatformDisclosure;
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

        group.MapGet("/highway-setback-standards", async (ISender sender) =>
            Results.Ok(await sender.Send(new GetHighwaySetbackStandardsQuery())))
            .WithName("AdminGetHighwaySetbackStandards");

        group.MapPut("/highway-setback-standards/{highwayFrontageType}", async (
            BhumiLogistics.Domain.Enums.HighwayType highwayFrontageType,
            UpdateSetbackRequest body,
            ISender sender) =>
        {
            await sender.Send(new UpdateHighwaySetbackStandardCommand(highwayFrontageType, body.SetbackDistanceInMeters, body.Notes));
            return Results.NoContent();
        })
        .WithName("AdminUpdateHighwaySetbackStandard")
        .Produces(StatusCodes.Status204NoContent);

        group.MapGet("/grievances", async (ISender sender) =>
         Results.Ok(await sender.Send(new GetAllGrievancesQuery())))
        .WithName("AdminGetAllGrievances");

        group.MapPut("/grievances/{id:guid}/resolve", async (Guid id, ResolveGrievanceRequest body, ISender sender) =>
        {
            await sender.Send(new ResolveGrievanceCommand(id, body.ResolutionNotes));
            return Results.NoContent();
        })
        .WithName("AdminResolveGrievance")
        .Produces(StatusCodes.Status204NoContent);

        group.MapPut("/platform-settings", async (UpdatePlatformSettingsCommand command, ISender sender) =>
        {
            await sender.Send(command);
            return Results.NoContent();
        })
        .WithName("AdminUpdatePlatformSettings")
        .Produces(StatusCodes.Status204NoContent);
    }
}
public sealed record VerifyOwnershipRequest(string? Notes);
public sealed record RejectOwnershipRequest(string Reason);
public sealed record UpdateSetbackRequest(decimal SetbackDistanceInMeters, string? Notes);
public sealed record ResolveGrievanceRequest(string ResolutionNotes);