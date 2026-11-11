using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// You may choose how to handle the reactivation charge for that subscription: 1) <c>prorated</c> A prorated charge for the product price will be attempted to complete the period 2) <c>immediate</c> A full-price charge for the product price will be attempted immediately 3) <c>delayed</c> A full-price charge for the product price will be attempted at the next renewal.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<ReactivationCharge>))]
public sealed record ReactivationCharge : OpenStringEnum<ReactivationCharge>
{
    private ReactivationCharge(string value) : base(value)
    {
    }

    public static readonly ReactivationCharge Prorated = new("prorated");

    public static readonly ReactivationCharge Immediate = new("immediate");

    public static readonly ReactivationCharge Delayed = new("delayed");

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
