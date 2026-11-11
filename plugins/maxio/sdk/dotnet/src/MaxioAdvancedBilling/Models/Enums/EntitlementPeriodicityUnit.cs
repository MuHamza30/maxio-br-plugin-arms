using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The recurring window over which a <c>usage_limit</c> feature's allowance resets.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<EntitlementPeriodicityUnit>))]
public sealed record EntitlementPeriodicityUnit : OpenStringEnum<EntitlementPeriodicityUnit>
{
    private EntitlementPeriodicityUnit(string value) : base(value)
    {
    }

    public static readonly EntitlementPeriodicityUnit Hour = new("hour");

    public static readonly EntitlementPeriodicityUnit Day = new("day");

    public static readonly EntitlementPeriodicityUnit Week = new("week");

    public static readonly EntitlementPeriodicityUnit Month = new("month");

    public static readonly EntitlementPeriodicityUnit Year = new("year");

    public TResult Match<TResult>(Func<TResult> onHour,
        Func<TResult> onDay,
        Func<TResult> onWeek,
        Func<TResult> onMonth,
        Func<TResult> onYear,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Hour => onHour(),
            _ when this == Day => onDay(),
            _ when this == Week => onWeek(),
            _ when this == Month => onMonth(),
            _ when this == Year => onYear(),
            _ => otherwise(Value)
        };

    public void Match(Action onHour,
        Action onDay,
        Action onWeek,
        Action onMonth,
        Action onYear,
        Action<string> otherwise)
    {
        if (this == Hour) onHour();
        else if (this == Day) onDay();
        else if (this == Week) onWeek();
        else if (this == Month) onMonth();
        else if (this == Year) onYear();
        else otherwise(Value);
    }
}
