using BhumiLogistics.Application.Features.Auth.Commands.Login;
using BhumiLogistics.Application.Features.Auth.Commands.Register;
using MediatR;

namespace BhumiLogistics.WebApi.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/register", async (RegisterCommand command, ISender sender) =>
        {
            var id = await sender.Send(command);
            return Results.Created($"/api/auth/{id}", new { id });
        })
        .WithName("Register")
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        group.MapPost("/login", async (LoginCommand command, ISender sender) =>
        {
            var token = await sender.Send(command);
            return Results.Ok(new { token });
        })
        .WithName("Login")
        .Produces(StatusCodes.Status200OK);
    }
}
