using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The data type of a feature's value. For <c>access_right</c> features this is always <c>boolean</c>, and for <c>usage_limit</c> features this is always <c>numeric</c>. For <c>service_right</c> features, you choose the value type explicitly.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<FeatureValueType>))]
public sealed record FeatureValueType : OpenStringEnum<FeatureValueType>
{
    private FeatureValueType(string value) : base(value)
    {
    }

    public static readonly FeatureValueType Text = new("text");

    public static readonly FeatureValueType Boolean = new("boolean");

    public static readonly FeatureValueType Numeric = new("numeric");

    public TResult Match<TResult>(Func<TResult> onText,
        Func<TResult> onBoolean,
        Func<TResult> onNumeric,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Text => onText(),
            _ when this == Boolean => onBoolean(),
            _ when this == Numeric => onNumeric(),
            _ => otherwise(Value)
        };

    public void Match(Action onText, Action onBoolean, Action onNumeric, Action<string> otherwise)
    {
        if (this == Text) onText();
        else if (this == Boolean) onBoolean();
        else if (this == Numeric) onNumeric();
        else otherwise(Value);
    }
}
