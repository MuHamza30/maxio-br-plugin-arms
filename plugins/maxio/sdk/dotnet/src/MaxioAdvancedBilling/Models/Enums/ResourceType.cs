using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<ResourceType>))]
public sealed record ResourceType : OpenStringEnum<ResourceType>
{
    private ResourceType(string value) : base(value)
    {
    }

    public static readonly ResourceType Subscriptions = new("subscriptions");

    public static readonly ResourceType Customers = new("customers");

    public TResult Match<TResult>(Func<TResult> onSubscriptions,
        Func<TResult> onCustomers,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Subscriptions => onSubscriptions(),
            _ when this == Customers => onCustomers(),
            _ => otherwise(Value)
        };

    public void Match(Action onSubscriptions, Action onCustomers, Action<string> otherwise)
    {
        if (this == Subscriptions) onSubscriptions();
        else if (this == Customers) onCustomers();
        else otherwise(Value);
    }
}
