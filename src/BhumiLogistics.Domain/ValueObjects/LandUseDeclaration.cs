using BhumiLogistics.Domain.Enums;
using BhumiLogistics.Domain.Exceptions;
using BhumiLogistics.Domain.Common;

namespace BhumiLogistics.Domain.ValueObjects;

/// <summary>
/// Captures a plot's official land-use category and, when that category isn't
/// already Commercial or Industrial, the local land-use council's conversion
/// approval required before the land can lawfully be used commercially.
/// </summary>
public sealed class LandUseDeclaration : ValueObject
{
    public LandUseClassification Classification { get; }
    public string? ConversionApprovalReferenceNumber { get; }
    public string? ConversionApprovingAuthority { get; }
    public DateOnly? ConversionApprovalDate { get; }

    private LandUseDeclaration(
        LandUseClassification classification,
        string? conversionApprovalReferenceNumber,
        string? conversionApprovingAuthority,
        DateOnly? conversionApprovalDate)
    {
        Classification = classification;
        ConversionApprovalReferenceNumber = conversionApprovalReferenceNumber;
        ConversionApprovingAuthority = conversionApprovingAuthority;
        ConversionApprovalDate = conversionApprovalDate;
    }

    public static LandUseDeclaration Create(
        LandUseClassification classification,
        string? conversionApprovalReferenceNumber,
        string? conversionApprovingAuthority,
        DateOnly? conversionApprovalDate)
    {
        var alreadyCommerciallyZoned =
            classification is LandUseClassification.Commercial or LandUseClassification.Industrial;

        if (!alreadyCommerciallyZoned)
        {
            if (string.IsNullOrWhiteSpace(conversionApprovalReferenceNumber))
                throw new DomainException(
                    "This land is not zoned Commercial or Industrial. A local land-use council " +
                    "conversion approval reference is required before it can be listed for commercial lease.");

            if (string.IsNullOrWhiteSpace(conversionApprovingAuthority))
                throw new DomainException("The approving land-use authority (e.g. the local land use council) is required.");

            if (conversionApprovalDate is null)
                throw new DomainException("The conversion approval date is required.");

            if (conversionApprovalDate > DateOnly.FromDateTime(DateTime.UtcNow))
                throw new DomainException("The conversion approval date cannot be in the future.");
        }

        return new LandUseDeclaration(
            classification, conversionApprovalReferenceNumber, conversionApprovingAuthority, conversionApprovalDate);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Classification;
        yield return ConversionApprovalReferenceNumber ?? string.Empty;
        yield return ConversionApprovingAuthority ?? string.Empty;
        yield return ConversionApprovalDate ?? default;
    }
}