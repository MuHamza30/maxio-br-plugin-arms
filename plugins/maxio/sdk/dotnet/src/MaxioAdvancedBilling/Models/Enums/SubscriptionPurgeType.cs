using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<SubscriptionPurgeType>))]
public sealed record SubscriptionPurgeType : OpenStringEnum<SubscriptionPurgeType>
{
    private SubscriptionPurgeType(string value) : base(value)
    {
    }

    public static readonly SubscriptionPurgeType Customer = new("customer");

    public static readonly SubscriptionPurgeType PaymentProfile = new("payment_profile");

    public TResult Match<TResult>(Func<TResult> onCustomer,
        Func<TResult> onPaymentProfile,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Customer => onCustomer(),
            _ when this == PaymentProfile => onPaymentProfile(),
            _ => otherwise(Value)
        };

    public void Match(Action onCustomer, Action onPaymentProfile, Action<string> otherwise)
    {
        if (this == Customer) onCustomer();
        else if (this == PaymentProfile) onPaymentProfile();
        else otherwise(Value);
    }
}
