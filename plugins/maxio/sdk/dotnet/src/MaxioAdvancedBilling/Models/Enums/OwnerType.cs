using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The polymorphic owner type. <c>subscription</c> is the only value currently supported.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<OwnerType>))]
public sealed record OwnerType : OpenStringEnum<OwnerType>
{
    private OwnerType(string value) : base(value)
    {
    }

    public static readonly OwnerType Subscription = new("subscription");

    public TResult Match<TResult>(Func<TResult> onSubscription, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Subscription => onSubscription(),
            _ => otherwise(Value)
        };

    public void Match(Action onSubscription, Action<string> otherwise)
    {
        if (this == Subscription) onSubscription();
        else otherwise(Value);
    }
}
