using BhumiLogistics.Domain.Common;
using BhumiLogistics.Domain.Exceptions;

namespace BhumiLogistics.Domain.ValueObjects;

/// <summary>
/// Immutable representation of a land parcel's size, expressed in the
/// traditional Bigha-Kattha unit system used for Terai commercial land.
/// </summary>
public sealed class LandArea : ValueObject
{
    private const int KatthaPerBigha = 20;

    public decimal SizeInBigha { get; }
    public decimal SizeInKattha { get; }

    private LandArea(decimal sizeInBigha, decimal sizeInKattha)
    {
        SizeInBigha = sizeInBigha;
        SizeInKattha = sizeInKattha;
    }

    public static LandArea Create(decimal bigha, decimal kattha)
    {
        if (bigha < 0 || kattha < 0)
            throw new InvalidLandAreaException("Land area components cannot be negative.");

        if (bigha == 0 && kattha == 0)
            throw new InvalidLandAreaException("Land area must be greater than zero.");

        return new LandArea(bigha, kattha);
    }

    /// <summary>Total normalized area expressed purely in Kattha, useful for sorting/filtering.</summary>
    public decimal ToTotalKattha() => (SizeInBigha * KatthaPerBigha) + SizeInKattha;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SizeInBigha;
        yield return SizeInKattha;
    }
}
