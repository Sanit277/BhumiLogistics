using BhumiLogistics.Domain.Common;
using BhumiLogistics.Domain.Exceptions;

namespace BhumiLogistics.Domain.Entities;

/// <summary>
/// Singleton configuration record holding the public disclosures required of
/// an e-commerce intermediary under the Electronic Commerce Act 2081 —
/// business registration details and a designated grievance officer's contact.
/// </summary>
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

    private PlatformSettings() { } // EF Core

    public PlatformSettings(
        string businessName, string panNumber, string? vatNumber, string registeredAddress,
        string contactEmail, string contactPhone,
        string grievanceOfficerName, string grievanceOfficerEmail, string grievanceOfficerPhone)
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
    }

    public void Update(
        string businessName, string panNumber, string? vatNumber, string registeredAddress,
        string contactEmail, string contactPhone,
        string grievanceOfficerName, string grievanceOfficerEmail, string grievanceOfficerPhone)
    {
        if (string.IsNullOrWhiteSpace(businessName))
            throw new DomainException("Business name is required.");

        if (string.IsNullOrWhiteSpace(panNumber))
            throw new DomainException("PAN number is required.");

        if (string.IsNullOrWhiteSpace(grievanceOfficerName) || string.IsNullOrWhiteSpace(grievanceOfficerEmail))
            throw new DomainException("A designated grievance officer's name and email are required.");

        BusinessName = businessName;
        PanNumber = panNumber;
        VatNumber = vatNumber;
        RegisteredAddress = registeredAddress;
        ContactEmail = contactEmail;
        ContactPhone = contactPhone;
        GrievanceOfficerName = grievanceOfficerName;
        GrievanceOfficerEmail = grievanceOfficerEmail;
        GrievanceOfficerPhone = grievanceOfficerPhone;
    }
}