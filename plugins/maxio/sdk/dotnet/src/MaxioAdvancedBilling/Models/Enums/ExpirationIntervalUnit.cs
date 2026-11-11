using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<ExpirationIntervalUnit>))]
public sealed record ExpirationIntervalUnit : OpenStringEnum<ExpirationIntervalUnit>
{
    private ExpirationIntervalUnit(string value) : base(value)
    {
    }

    public static readonly ExpirationIntervalUnit Day = new("day");

    public static readonly ExpirationIntervalUnit Month = new("month");

    public static readonly ExpirationIntervalUnit Never = new("never");

    public TResult Match<TResult>(Func<TResult> onDay,
        Func<TResult> onMonth,
        Func<TResult> onNever,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Day => onDay(),
            _ when this == Month => onMonth(),
            _ when this == Never => onNever(),
            _ => otherwise(Value)
        };

    public void Match(Action onDay, Action onMonth, Action onNever, Action<string> otherwise)
    {
        if (this == Day) onDay();
        else if (this == Month) onMonth();
        else if (this == Never) onNever();
        else otherwise(Value);
    }
}
