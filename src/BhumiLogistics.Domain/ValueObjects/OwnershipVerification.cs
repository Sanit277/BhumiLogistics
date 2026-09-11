using BhumiLogistics.Domain.Enums;
using BhumiLogistics.Domain.Exceptions;

namespace BhumiLogistics.Domain.ValueObjects;

/// <summary>
/// Captures everything needed to verify that a listed plot is legally leasable:
/// the registered owner's identity, the relationship of the account holder to
/// that owner, tenure type, and Mohi (tenant-farmer) status. Unlike other value
/// objects in this domain, this one carries a mutable review workflow, since an
/// admin transitions it from PendingReview to Verified or Rejected over time.
/// </summary>
public class OwnershipVerification
{
    public OwnershipVerificationStatus Status { get; private set; }
    public string RegisteredOwnerName { get; private set; } = string.Empty;
    public OwnerRelationship RelationshipToOwner { get; private set; }
    public string? PowerOfAttorneyReferenceNumber { get; private set; }
    public string LalpurjaReferenceNumber { get; private set; } = string.Empty;
    public string KittaNumber { get; private set; } = string.Empty;
    public string WardMunicipality { get; private set; } = string.Empty;
    public string? LandIdentityNumber { get; private set; }
    public LandTenureType TenureType { get; private set; }
    public bool MohiTenancyDeclared { get; private set; }
    public string? MohiTenancyNotes { get; private set; }
    public Guid? VerifiedByUserId { get; private set; }
    public DateTimeOffset? VerifiedAtUtc { get; private set; }
    public string? VerificationNotes { get; private set; }

    private OwnershipVerification() { } // EF Core

    private OwnershipVerification(
        string registeredOwnerName,
        OwnerRelationship relationshipToOwner,
        string? powerOfAttorneyReferenceNumber,
        string lalpurjaReferenceNumber,
        string kittaNumber,
        string wardMunicipality,
        string? landIdentityNumber,
        LandTenureType tenureType,
        bool mohiTenancyDeclared,
        string? mohiTenancyNotes)
    {
        RegisteredOwnerName = registeredOwnerName;
        RelationshipToOwner = relationshipToOwner;
        PowerOfAttorneyReferenceNumber = powerOfAttorneyReferenceNumber;
        LalpurjaReferenceNumber = lalpurjaReferenceNumber;
        KittaNumber = kittaNumber;
        WardMunicipality = wardMunicipality;
        LandIdentityNumber = landIdentityNumber;
        TenureType = tenureType;
        MohiTenancyDeclared = mohiTenancyDeclared;
        MohiTenancyNotes = mohiTenancyNotes;
        Status = OwnershipVerificationStatus.PendingReview;
    }

    public static OwnershipVerification Declare(
        string registeredOwnerName,
        OwnerRelationship relationshipToOwner,
        string? powerOfAttorneyReferenceNumber,
        string lalpurjaReferenceNumber,
        string kittaNumber,
        string wardMunicipality,
        string? landIdentityNumber,
        LandTenureType tenureType,
        bool mohiTenancyDeclared,
        string? mohiTenancyNotes)
    {
        if (string.IsNullOrWhiteSpace(registeredOwnerName))
            throw new DomainException("The registered owner's name (as shown on the Lalpurja) is required.");

        if (string.IsNullOrWhiteSpace(lalpurjaReferenceNumber))
            throw new DomainException("A Lalpurja reference number is required to list a plot.");

        if (string.IsNullOrWhiteSpace(kittaNumber))
            throw new DomainException("The Kitta (plot) number is required to list a plot.");

        if (string.IsNullOrWhiteSpace(wardMunicipality))
            throw new DomainException("The ward/municipality of the plot is required.");

        if (relationshipToOwner == OwnerRelationship.AuthorizedAgent
            && string.IsNullOrWhiteSpace(powerOfAttorneyReferenceNumber))
            throw new DomainException(
                "A Power of Attorney reference is required when listing on behalf of the registered owner.");

        if (tenureType is LandTenureType.GuthiOther or LandTenureType.Government)
            throw new DomainException(
                "Land of this tenure type cannot be listed on this platform, as it is not privately leasable under Nepali law.");

        return new OwnershipVerification(
            registeredOwnerName, relationshipToOwner, powerOfAttorneyReferenceNumber,
            lalpurjaReferenceNumber, kittaNumber, wardMunicipality, landIdentityNumber,
            tenureType, mohiTenancyDeclared, mohiTenancyNotes);
    }

    public void MarkVerified(Guid adminUserId, string? notes)
    {
        Status = OwnershipVerificationStatus.Verified;
        VerifiedByUserId = adminUserId;
        VerifiedAtUtc = DateTimeOffset.UtcNow;
        VerificationNotes = notes;
    }

    public void Reject(Guid adminUserId, string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
            throw new DomainException("A reason is required when rejecting a plot's ownership verification.");

        Status = OwnershipVerificationStatus.Rejected;
        VerifiedByUserId = adminUserId;
        VerifiedAtUtc = DateTimeOffset.UtcNow;
        VerificationNotes = reason;
    }
}