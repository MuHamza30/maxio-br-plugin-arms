using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// Determines the direction of the movement. The amount itself is always sent positive.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<Kind1>))]
public sealed record Kind1 : OpenStringEnum<Kind1>
{
    private Kind1(string value) : base(value)
    {
    }

    public static readonly Kind1 Grant = new("grant");

    public static readonly Kind1 Debit = new("debit");

    public TResult Match<TResult>(Func<TResult> onGrant, Func<TResult> onDebit, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Grant => onGrant(),
            _ when this == Debit => onDebit(),
            _ => otherwise(Value)
        };

    public void Match(Action onGrant, Action onDebit, Action<string> otherwise)
    {
        if (this == Grant) onGrant();
        else if (this == Debit) onDebit();
        else otherwise(Value);
    }
}
