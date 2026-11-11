using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<QScope>))]
public sealed record QScope : OpenStringEnum<QScope>
{
    private QScope(string value) : base(value)
    {
    }

    public static readonly QScope FullName = new("full_name");

    public static readonly QScope FirstName = new("first_name");

    public static readonly QScope LastName = new("last_name");

    public static readonly QScope Organization = new("organization");

    public static readonly QScope CustomerReference = new("customer_reference");

    public static readonly QScope SubscriptionReference = new("subscription_reference");

    public TResult Match<TResult>(Func<TResult> onFullName,
        Func<TResult> onFirstName,
        Func<TResult> onLastName,
        Func<TResult> onOrganization,
        Func<TResult> onCustomerReference,
        Func<TResult> onSubscriptionReference,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == FullName => onFullName(),
            _ when this == FirstName => onFirstName(),
            _ when this == LastName => onLastName(),
            _ when this == Organization => onOrganization(),
            _ when this == CustomerReference => onCustomerReference(),
            _ when this == SubscriptionReference => onSubscriptionReference(),
            _ => otherwise(Value)
        };

    public void Match(Action onFullName,
        Action onFirstName,
        Action onLastName,
        Action onOrganization,
        Action onCustomerReference,
        Action onSubscriptionReference,
        Action<string> otherwise)
    {
        if (this == FullName) onFullName();
        else if (this == FirstName) onFirstName();
        else if (this == LastName) onLastName();
        else if (this == Organization) onOrganization();
        else if (this == CustomerReference) onCustomerReference();
        else if (this == SubscriptionReference) onSubscriptionReference();
        else otherwise(Value);
    }
}
