using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<DiscountType>))]
public sealed record DiscountType : OpenStringEnum<DiscountType>
{
    private DiscountType(string value) : base(value)
    {
    }

    public static readonly DiscountType Amount = new("amount");

    public static readonly DiscountType Percent = new("percent");

    public TResult Match<TResult>(Func<TResult> onAmount, Func<TResult> onPercent, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Amount => onAmount(),
            _ when this == Percent => onPercent(),
            _ => otherwise(Value)
        };

    public void Match(Action onAmount, Action onPercent, Action<string> otherwise)
    {
        if (this == Amount) onAmount();
        else if (this == Percent) onPercent();
        else otherwise(Value);
    }
}
