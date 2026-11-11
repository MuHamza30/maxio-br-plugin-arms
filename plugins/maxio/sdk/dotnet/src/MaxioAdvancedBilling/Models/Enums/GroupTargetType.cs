using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The type of object indicated by the id attribute.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<GroupTargetType>))]
public sealed record GroupTargetType : OpenStringEnum<GroupTargetType>
{
    private GroupTargetType(string value) : base(value)
    {
    }

    public static readonly GroupTargetType Customer = new("customer");

    public static readonly GroupTargetType Subscription = new("subscription");

    public static readonly GroupTargetType Self = new("self");

    public static readonly GroupTargetType Parent = new("parent");

    public static readonly GroupTargetType Eldest = new("eldest");

    public TResult Match<TResult>(Func<TResult> onCustomer,
        Func<TResult> onSubscription,
        Func<TResult> onSelf,
        Func<TResult> onParent,
        Func<TResult> onEldest,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Customer => onCustomer(),
            _ when this == Subscription => onSubscription(),
            _ when this == Self => onSelf(),
            _ when this == Parent => onParent(),
            _ when this == Eldest => onEldest(),
            _ => otherwise(Value)
        };

    public void Match(Action onCustomer,
        Action onSubscription,
        Action onSelf,
        Action onParent,
        Action onEldest,
        Action<string> otherwise)
    {
        if (this == Customer) onCustomer();
        else if (this == Subscription) onSubscription();
        else if (this == Self) onSelf();
        else if (this == Parent) onParent();
        else if (this == Eldest) onEldest();
        else otherwise(Value);
    }
}
