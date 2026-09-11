namespace BhumiLogistics.Domain.Enums;

/// <summary>
/// Nepal's land tenure categories. GuthiOther and Government are excluded from this
/// platform entirely — they are largely non-transferable/non-leasable by private landowners.
/// </summary>
public enum LandTenureType
{
    Raikar = 0,
    GuthiRaitanNumbari = 1,
    GuthiOther = 2,
    Government = 3,
    Unknown = 4
}