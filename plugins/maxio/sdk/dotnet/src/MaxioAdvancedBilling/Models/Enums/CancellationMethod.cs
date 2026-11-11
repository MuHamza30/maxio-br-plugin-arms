using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The process used to cancel the subscription, if the subscription has been canceled. It is nil if the subscription's state is not canceled.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<CancellationMethod>))]
public sealed record CancellationMethod : OpenStringEnum<CancellationMethod>
{
    private CancellationMethod(string value) : base(value)
    {
    }

    public static readonly CancellationMethod MerchantUi = new("merchant_ui");

    public static readonly CancellationMethod MerchantApi = new("merchant_api");

    public static readonly CancellationMethod Dunning = new("dunning");

    public static readonly CancellationMethod BillingPortal = new("billing_portal");

    public static readonly CancellationMethod Unknown = new("unknown");

    public static readonly CancellationMethod Imported = new("imported");

    public TResult Match<TResult>(Func<TResult> onMerchantUi,
        Func<TResult> onMerchantApi,
        Func<TResult> onDunning,
        Func<TResult> onBillingPortal,
        Func<TResult> onUnknown,
        Func<TResult> onImported,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == MerchantUi => onMerchantUi(),
            _ when this == MerchantApi => onMerchantApi(),
            _ when this == Dunning => onDunning(),
            _ when this == BillingPortal => onBillingPortal(),
            _ when this == Unknown => onUnknown(),
            _ when this == Imported => onImported(),
            _ => otherwise(Value)
        };

    public void Match(Action onMerchantUi,
        Action onMerchantApi,
        Action onDunning,
        Action onBillingPortal,
        Action onUnknown,
        Action onImported,
        Action<string> otherwise)
    {
        if (this == MerchantUi) onMerchantUi();
        else if (this == MerchantApi) onMerchantApi();
        else if (this == Dunning) onDunning();
        else if (this == BillingPortal) onBillingPortal();
        else if (this == Unknown) onUnknown();
        else if (this == Imported) onImported();
        else otherwise(Value);
    }
}
