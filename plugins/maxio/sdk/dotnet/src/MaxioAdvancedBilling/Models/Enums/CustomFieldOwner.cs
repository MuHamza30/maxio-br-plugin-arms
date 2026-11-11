using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<CustomFieldOwner>))]
public sealed record CustomFieldOwner : OpenStringEnum<CustomFieldOwner>
{
    private CustomFieldOwner(string value) : base(value)
    {
    }

    public static readonly CustomFieldOwner Customer = new("Customer");

    public static readonly CustomFieldOwner Subscription = new("Subscription");

    public TResult Match<TResult>(Func<TResult> onCustomer,
        Func<TResult> onSubscription,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Customer => onCustomer(),
            _ when this == Subscription => onSubscription(),
            _ => otherwise(Value)
        };

    public void Match(Action onCustomer, Action onSubscription, Action<string> otherwise)
    {
        if (this == Customer) onCustomer();
        else if (this == Subscription) onSubscription();
        else otherwise(Value);
    }
}
