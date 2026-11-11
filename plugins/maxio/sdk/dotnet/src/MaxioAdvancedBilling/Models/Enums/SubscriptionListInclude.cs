using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<SubscriptionListInclude>))]
public sealed record SubscriptionListInclude : OpenStringEnum<SubscriptionListInclude>
{
    private SubscriptionListInclude(string value) : base(value)
    {
    }

    public static readonly SubscriptionListInclude SelfServicePageToken = new("self_service_page_token");

    public static readonly SubscriptionListInclude CurrentAccountBalanceInCents = new(
        "current_account_balance_in_cents");

    public static readonly SubscriptionListInclude CurrentBillingAmount = new("current_billing_amount");

    public static readonly SubscriptionListInclude Coupons = new("coupons");

    public TResult Match<TResult>(Func<TResult> onSelfServicePageToken,
        Func<TResult> onCurrentAccountBalanceInCents,
        Func<TResult> onCurrentBillingAmount,
        Func<TResult> onCoupons,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == SelfServicePageToken => onSelfServicePageToken(),
            _ when this == CurrentAccountBalanceInCents => onCurrentAccountBalanceInCents(),
            _ when this == CurrentBillingAmount => onCurrentBillingAmount(),
            _ when this == Coupons => onCoupons(),
            _ => otherwise(Value)
        };

    public void Match(Action onSelfServicePageToken,
        Action onCurrentAccountBalanceInCents,
        Action onCurrentBillingAmount,
        Action onCoupons,
        Action<string> otherwise)
    {
        if (this == SelfServicePageToken) onSelfServicePageToken();
        else if (this == CurrentAccountBalanceInCents) onCurrentAccountBalanceInCents();
        else if (this == CurrentBillingAmount) onCurrentBillingAmount();
        else if (this == Coupons) onCoupons();
        else otherwise(Value);
    }
}
