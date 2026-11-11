using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<InvoiceDiscountType>))]
public sealed record InvoiceDiscountType : OpenStringEnum<InvoiceDiscountType>
{
    private InvoiceDiscountType(string value) : base(value)
    {
    }

    public static readonly InvoiceDiscountType Percentage = new("percentage");

    public static readonly InvoiceDiscountType FlatAmount = new("flat_amount");

    public static readonly InvoiceDiscountType Rollover = new("rollover");

    public TResult Match<TResult>(Func<TResult> onPercentage,
        Func<TResult> onFlatAmount,
        Func<TResult> onRollover,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Percentage => onPercentage(),
            _ when this == FlatAmount => onFlatAmount(),
            _ when this == Rollover => onRollover(),
            _ => otherwise(Value)
        };

    public void Match(Action onPercentage, Action onFlatAmount, Action onRollover, Action<string> otherwise)
    {
        if (this == Percentage) onPercentage();
        else if (this == FlatAmount) onFlatAmount();
        else if (this == Rollover) onRollover();
        else otherwise(Value);
    }
}
