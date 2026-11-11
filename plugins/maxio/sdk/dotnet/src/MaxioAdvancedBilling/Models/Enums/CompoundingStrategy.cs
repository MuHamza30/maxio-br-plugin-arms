using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// Applicable only to stackable coupons. For <c>compound</c>, Percentage-based discounts will be calculated against the remaining price, after prior discounts have been calculated. For <c>full-price</c>, Percentage-based discounts will always be calculated against the original item price, before other discounts are applied.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<CompoundingStrategy>))]
public sealed record CompoundingStrategy : OpenStringEnum<CompoundingStrategy>
{
    private CompoundingStrategy(string value) : base(value)
    {
    }

    public static readonly CompoundingStrategy Compound = new("compound");

    public static readonly CompoundingStrategy FullPrice = new("full-price");

    public TResult Match<TResult>(Func<TResult> onCompound,
        Func<TResult> onFullPrice,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Compound => onCompound(),
            _ when this == FullPrice => onFullPrice(),
            _ => otherwise(Value)
        };

    public void Match(Action onCompound, Action onFullPrice, Action<string> otherwise)
    {
        if (this == Compound) onCompound();
        else if (this == FullPrice) onFullPrice();
        else otherwise(Value);
    }
}
