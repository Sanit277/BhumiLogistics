using BhumiLogistics.Domain.Enums;

namespace BhumiLogistics.Application.Features.Admin.Queries.GetAllLeaseOffers;

public sealed record AdminLeaseOfferDto(
    Guid Id,
    Guid LandPlotId,
    string PlotPlusCode,
    string TenantName,
    decimal OfferedAmount,
    int DurationInYears,
    OfferStatus Status,
    DateOnly ProposedStartDate,
    MalpotRegistrationStatus MalpotRegistrationStatus,
    string? RegisteredDeedReferenceNumber,
    DateOnly? RegistrationDate,
    OwnerType? OwnerType,
    decimal EstimatedTdsWithholdingAmount,
    DateTimeOffset CreatedAtUtc);