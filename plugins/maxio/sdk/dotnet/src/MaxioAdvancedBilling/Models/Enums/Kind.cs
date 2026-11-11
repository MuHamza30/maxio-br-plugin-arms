using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<Kind>))]
public sealed record Kind : OpenStringEnum<Kind>
{
    private Kind(string value) : base(value)
    {
    }

    public static readonly Kind AccessRight = new("access_right");

    public static readonly Kind UsageLimit = new("usage_limit");

    public static readonly Kind ServiceRight = new("service_right");

    public static readonly Kind All = new("all");

    public TResult Match<TResult>(Func<TResult> onAccessRight,
        Func<TResult> onUsageLimit,
        Func<TResult> onServiceRight,
        Func<TResult> onAll,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == AccessRight => onAccessRight(),
            _ when this == UsageLimit => onUsageLimit(),
            _ when this == ServiceRight => onServiceRight(),
            _ when this == All => onAll(),
            _ => otherwise(Value)
        };

    public void Match(Action onAccessRight,
        Action onUsageLimit,
        Action onServiceRight,
        Action onAll,
        Action<string> otherwise)
    {
        if (this == AccessRight) onAccessRight();
        else if (this == UsageLimit) onUsageLimit();
        else if (this == ServiceRight) onServiceRight();
        else if (this == All) onAll();
        else otherwise(Value);
    }
}
