using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<SubscriptionGroupInclude>))]
public sealed record SubscriptionGroupInclude : OpenStringEnum<SubscriptionGroupInclude>
{
    private SubscriptionGroupInclude(string value) : base(value)
    {
    }

    public static readonly SubscriptionGroupInclude CurrentBillingAmountInCents = new(
        "current_billing_amount_in_cents");

    public TResult Match<TResult>(Func<TResult> onCurrentBillingAmountInCents, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == CurrentBillingAmountInCents => onCurrentBillingAmountInCents(),
            _ => otherwise(Value)
        };

    public void Match(Action onCurrentBillingAmountInCents, Action<string> otherwise)
    {
        if (this == CurrentBillingAmountInCents) onCurrentBillingAmountInCents();
        else otherwise(Value);
    }
}
