using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<SubscriptionInclude>))]
public sealed record SubscriptionInclude : OpenStringEnum<SubscriptionInclude>
{
    private SubscriptionInclude(string value) : base(value)
    {
    }

    public static readonly SubscriptionInclude Coupons = new("coupons");

    public static readonly SubscriptionInclude SelfServicePageToken = new("self_service_page_token");

    public TResult Match<TResult>(Func<TResult> onCoupons,
        Func<TResult> onSelfServicePageToken,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Coupons => onCoupons(),
            _ when this == SelfServicePageToken => onSelfServicePageToken(),
            _ => otherwise(Value)
        };

    public void Match(Action onCoupons, Action onSelfServicePageToken, Action<string> otherwise)
    {
        if (this == Coupons) onCoupons();
        else if (this == SelfServicePageToken) onSelfServicePageToken();
        else otherwise(Value);
    }
}
