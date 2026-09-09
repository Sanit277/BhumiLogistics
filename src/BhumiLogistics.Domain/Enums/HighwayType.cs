namespace BhumiLogistics.Domain.Enums;

/// <summary>
/// Describes the nature of a land plot's frontage relative to a highway —
/// a key commercial valuation driver for logistics/warehousing tenants.
/// </summary>
public enum HighwayType
{
    None = 0,
    StateHighwayFrontage = 1,
    NationalHighwayFrontage = 2,
    ExpresswayFrontage = 3,
    CornerPlotDualFrontage = 4
}
