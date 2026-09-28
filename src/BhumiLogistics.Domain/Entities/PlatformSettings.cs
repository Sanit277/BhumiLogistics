using BhumiLogistics.Domain.Common;
using BhumiLogistics.Domain.Exceptions;

namespace BhumiLogistics.Domain.Entities;

public class PlatformSettings : BaseAuditableEntity
{
    public string BusinessName { get; private set; } = string.Empty;
    public string PanNumber { get; private set; } = string.Empty;
    public string? VatNumber { get; private set; }
    public string RegisteredAddress { get; private set; } = string.Empty;
    public string ContactEmail { get; private set; } = string.Empty;
    public string ContactPhone { get; private set; } = string.Empty;
    public string GrievanceOfficerName { get; private set; } = string.Empty;
    public string GrievanceOfficerEmail { get; private set; } = string.Empty;
    public string GrievanceOfficerPhone { get; private set; } = string.Empty;

    /// <summary>
    /// A review-trigger threshold, not an authoritative legal ceiling (the real
    /// Land Act ceiling varies by district and land type). Owners whose declared
    /// or platform-verified holdings exceed this get flagged for manual review.
    /// </summary>
    public decimal LandCeilingReviewThresholdInKattha { get; private set; }

    private PlatformSettings() { } // EF Core

    public PlatformSettings(
        string businessName, string panNumber, string? vatNumber, string registeredAddress,
        string contactEmail, string contactPhone,
        string grievanceOfficerName, string grievanceOfficerEmail, string grievanceOfficerPhone,
        decimal landCeilingReviewThresholdInKattha)
    {
        BusinessName = businessName;
        PanNumber = panNumber;
        VatNumber = vatNumber;
        RegisteredAddress = registeredAddress;
        ContactEmail = contactEmail;
        ContactPhone = contactPhone;
        GrievanceOfficerName = grievanceOfficerName;
        GrievanceOfficerEmail = grievanceOfficerEmail;
        GrievanceOfficerPhone = grievanceOfficerPhone;
        LandCeilingReviewThresholdInKattha = landCeilingReviewThresholdInKattha;
    }

    public void Update(
        string businessName, string panNumber, string? vatNumber, string registeredAddress,
        string contactEmail, string contactPhone,
        string grievanceOfficerName, string grievanceOfficerEmail, string grievanceOfficerPhone,
        decimal landCeilingReviewThresholdInKattha)
    {
        if (string.IsNullOrWhiteSpace(businessName))
            throw new DomainException("Business name is required.");

        if (string.IsNullOrWhiteSpace(panNumber))
            throw new DomainException("PAN number is required.");

        if (string.IsNullOrWhiteSpace(grievanceOfficerName) || string.IsNullOrWhiteSpace(grievanceOfficerEmail))
            throw new DomainException("A designated grievance officer's name and email are required.");

        if (landCeilingReviewThresholdInKattha <= 0)
            throw new DomainException("The land ceiling review threshold must be greater than zero.");

        BusinessName = businessName;
        PanNumber = panNumber;
        VatNumber = vatNumber;
        RegisteredAddress = registeredAddress;
        ContactEmail = contactEmail;
        ContactPhone = contactPhone;
        GrievanceOfficerName = grievanceOfficerName;
        GrievanceOfficerEmail = grievanceOfficerEmail;
        GrievanceOfficerPhone = grievanceOfficerPhone;
        LandCeilingReviewThresholdInKattha = landCeilingReviewThresholdInKattha;
    }
}