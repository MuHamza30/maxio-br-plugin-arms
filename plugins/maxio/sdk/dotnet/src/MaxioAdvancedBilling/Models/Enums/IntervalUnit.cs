using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<IntervalUnit>))]
public sealed record IntervalUnit : OpenStringEnum<IntervalUnit>
{
    private IntervalUnit(string value) : base(value)
    {
    }

    public static readonly IntervalUnit Day = new("day");

    public static readonly IntervalUnit Month = new("month");

    public TResult Match<TResult>(Func<TResult> onDay, Func<TResult> onMonth, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Day => onDay(),
            _ when this == Month => onMonth(),
            _ => otherwise(Value)
        };

    public void Match(Action onDay, Action onMonth, Action<string> otherwise)
    {
        if (this == Day) onDay();
        else if (this == Month) onMonth();
        else otherwise(Value);
    }
}
