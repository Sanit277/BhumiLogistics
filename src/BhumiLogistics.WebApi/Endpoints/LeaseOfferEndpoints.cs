using BhumiLogistics.Application.Features.LeaseOffers.Commands.AcceptLeaseOffer;
using BhumiLogistics.Application.Features.LeaseOffers.Commands.SubmitLeaseOffer;
using BhumiLogistics.Application.Features.LeaseOffers.Commands.MarkLeasePendingRegistration;
using BhumiLogistics.Application.Features.LeaseOffers.Commands.RegisterLease;
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
        group.MapPut("/{leaseOfferId:guid}/mark-pending-registration", async (Guid leaseOfferId, ISender sender) =>
        {
            await sender.Send(new MarkLeasePendingRegistrationCommand(leaseOfferId));
            return Results.NoContent();
        })
        .WithName("MarkLeasePendingRegistration")
        .RequireAuthorization(policy => policy.RequireRole("Landowner"))
        .Produces(StatusCodes.Status204NoContent);

        group.MapPut("/{leaseOfferId:guid}/register", async (
            Guid leaseOfferId, RegisterLeaseRequest body, ISender sender) =>
        {
            await sender.Send(new RegisterLeaseCommand(leaseOfferId, body.DeedReferenceNumber, body.RegistrationDate));
            return Results.NoContent();
        })
        .WithName("RegisterLease")
        .RequireAuthorization(policy => policy.RequireRole("Landowner"))
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem();
    }
}

/// <summary>Request body for accepting a lease offer — pairs the offer with its parent plot.</summary>
public sealed record AcceptLeaseOfferRequest(Guid LandPlotId);
public sealed record RegisterLeaseRequest(string DeedReferenceNumber, DateOnly RegistrationDate);
