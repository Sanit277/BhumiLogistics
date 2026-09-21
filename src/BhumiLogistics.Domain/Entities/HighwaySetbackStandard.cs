using BhumiLogistics.Domain.Common;
using BhumiLogistics.Domain.Enums;
using BhumiLogistics.Domain.Exceptions;

namespace BhumiLogistics.Domain.Entities;

/// <summary>
/// Admin-editable reference data mapping a highway frontage type to its current
/// statutory right-of-way/setback distance. Deliberately stored in the database
/// rather than hardcoded, since these distances are set by regulation that is
/// currently in flux (a draft bill would revise several of these figures).
/// </summary>
public class HighwaySetbackStandard : BaseEntity
{
    public HighwayType HighwayFrontageType { get; private set; }
    public decimal SetbackDistanceInMeters { get; private set; }
    public string? Notes { get; private set; }

    private HighwaySetbackStandard() { } // EF Core

    public HighwaySetbackStandard(HighwayType highwayFrontageType, decimal setbackDistanceInMeters, string? notes)
    {
        HighwayFrontageType = highwayFrontageType;
        SetbackDistanceInMeters = setbackDistanceInMeters;
        Notes = notes;
    }

    public void UpdateSetbackDistance(decimal newDistanceInMeters, string? notes)
    {
        if (newDistanceInMeters < 0)
            throw new DomainException("Setback distance cannot be negative.");

        SetbackDistanceInMeters = newDistanceInMeters;
        Notes = notes;
    }
}