using BhumiLogistics.Application.Features.Platform.Queries.GetPlatformDisclosure;
using MediatR;

namespace BhumiLogistics.WebApi.Endpoints;

/// <summary>
/// Public disclosure endpoint required under the Electronic Commerce Act 2081.
/// Deliberately has no .RequireAuthorization() — this must be visible to anyone.
/// </summary>
public static class PlatformEndpoints
{
    public static void MapPlatformEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/platform-disclosure", async (ISender sender) =>
            Results.Ok(await sender.Send(new GetPlatformDisclosureQuery())))
            .WithName("GetPlatformDisclosure")
            .WithTags("Platform")
            .Produces<PlatformDisclosureDto>();
    }
}