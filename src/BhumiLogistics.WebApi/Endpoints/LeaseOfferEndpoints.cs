using BhumiLogistics.Application.Features.LeaseOffers.Commands.AcceptLeaseOffer;
using BhumiLogistics.Application.Features.LeaseOffers.Commands.SubmitLeaseOffer;
using MediatR;

namespace BhumiLogistics.WebApi.Endpoints;

public static class LeaseOfferEndpoints
{
    public static void MapLeaseOfferEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/lease-offers").WithTags("Lease Offers");

    

        group.MapPost("/", async (SubmitLeaseOfferCommand command, ISender sender) =>
        {
            var id = await sender.Send(command);
            return Results.Created($"/api/lease-offers/{id}", new { id });
        })
        .WithName("SubmitLeaseOffer")
        .RequireAuthorization(policy => policy.RequireRole("CorporateTenant"))   // ← CHANGED
        .Produces(StatusCodes.Status201Created)
        .ProducesValidationProblem();

        group.MapPut("/{leaseOfferId:guid}/accept", async (
            Guid leaseOfferId, AcceptLeaseOfferRequest body, ISender sender) =>
        {
            await sender.Send(new AcceptLeaseOfferCommand(body.LandPlotId, leaseOfferId));
            return Results.NoContent();
        })
        .WithName("AcceptLeaseOffer")
        .RequireAuthorization(policy => policy.RequireRole("Landowner"))   // ← CHANGED
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem();
    }
}

/// <summary>Request body for accepting a lease offer — pairs the offer with its parent plot.</summary>
public sealed record AcceptLeaseOfferRequest(Guid LandPlotId);
