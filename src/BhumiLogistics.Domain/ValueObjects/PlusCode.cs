using System.Text.RegularExpressions;
using BhumiLogistics.Domain.Common;
using BhumiLogistics.Domain.Exceptions;

namespace BhumiLogistics.Domain.ValueObjects;

/// <summary>
/// Represents a Google Plus Code (Open Location Code) used to geo-tag a land
/// plot when a formal street address does not exist — common for rural highway land.
/// </summary>
public sealed partial class PlusCode : ValueObject
{
    public string Value { get; }

    private PlusCode(string value) => Value = value;

    public static PlusCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Plus Code cannot be empty.");

        if (!PlusCodePattern().IsMatch(value.Trim().ToUpperInvariant()))
            throw new DomainException($"'{value}' is not a valid Plus Code format.");

        return new PlusCode(value.Trim().ToUpperInvariant());
    }

    [GeneratedRegex(@"^[23456789CFGHJMPQRVWX]{4,8}\+[23456789CFGHJMPQRVWX]{2,3}$")]
    private static partial Regex PlusCodePattern();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
