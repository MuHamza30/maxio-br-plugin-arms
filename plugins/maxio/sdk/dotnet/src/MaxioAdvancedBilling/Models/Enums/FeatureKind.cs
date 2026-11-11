using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The behavior of a feature:
/// - <c>access_right</c>: a boolean entitlement. A subscriber either has access or does not.
/// - <c>usage_limit</c>: a quantified allowance measured over a recurring period (for example, "10,000 API calls per month").
/// - <c>service_right</c>: a free-form value (text, boolean, or number) that isn't a simple access flag or a metered limit.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<FeatureKind>))]
public sealed record FeatureKind : OpenStringEnum<FeatureKind>
{
    private FeatureKind(string value) : base(value)
    {
    }

    public static readonly FeatureKind AccessRight = new("access_right");

    public static readonly FeatureKind UsageLimit = new("usage_limit");

    public static readonly FeatureKind ServiceRight = new("service_right");

    public TResult Match<TResult>(Func<TResult> onAccessRight,
        Func<TResult> onUsageLimit,
        Func<TResult> onServiceRight,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == AccessRight => onAccessRight(),
            _ when this == UsageLimit => onUsageLimit(),
            _ when this == ServiceRight => onServiceRight(),
            _ => otherwise(Value)
        };

    public void Match(Action onAccessRight, Action onUsageLimit, Action onServiceRight, Action<string> otherwise)
    {
        if (this == AccessRight) onAccessRight();
        else if (this == UsageLimit) onUsageLimit();
        else if (this == ServiceRight) onServiceRight();
        else otherwise(Value);
    }
}
