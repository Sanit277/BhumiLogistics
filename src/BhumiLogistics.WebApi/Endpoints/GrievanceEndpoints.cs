using BhumiLogistics.Application.Features.Grievances.Commands.SubmitGrievance;
using BhumiLogistics.Application.Features.Grievances.Queries.GetMyGrievances;
using MediatR;

namespace BhumiLogistics.WebApi.Endpoints;

public static class GrievanceEndpoints
{
    public static void MapGrievanceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/grievances").WithTags("Grievances").RequireAuthorization();

        group.MapPost("/", async (SubmitGrievanceCommand command, ISender sender) =>
        {
            var id = await sender.Send(command);
            return Results.Created($"/api/grievances/{id}", new { id });
        })
        .WithName("SubmitGrievance")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        group.MapGet("/mine", async (ISender sender) =>
            Results.Ok(await sender.Send(new GetMyGrievancesQuery())))
            .WithName("GetMyGrievances");
    }
}