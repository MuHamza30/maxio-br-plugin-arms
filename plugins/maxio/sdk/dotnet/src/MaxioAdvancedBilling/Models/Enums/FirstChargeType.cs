using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<FirstChargeType>))]
public sealed record FirstChargeType : OpenStringEnum<FirstChargeType>
{
    private FirstChargeType(string value) : base(value)
    {
    }

    public static readonly FirstChargeType Prorated = new("prorated");

    public static readonly FirstChargeType Immediate = new("immediate");

    public static readonly FirstChargeType Delayed = new("delayed");

    public TResult Match<TResult>(Func<TResult> onProrated,
        Func<TResult> onImmediate,
        Func<TResult> onDelayed,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Prorated => onProrated(),
            _ when this == Immediate => onImmediate(),
            _ when this == Delayed => onDelayed(),
            _ => otherwise(Value)
        };

    public void Match(Action onProrated, Action onImmediate, Action onDelayed, Action<string> otherwise)
    {
        if (this == Prorated) onProrated();
        else if (this == Immediate) onImmediate();
        else if (this == Delayed) onDelayed();
        else otherwise(Value);
    }
}
