using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<TaxDestinationAddress>))]
public sealed record TaxDestinationAddress : OpenStringEnum<TaxDestinationAddress>
{
    private TaxDestinationAddress(string value) : base(value)
    {
    }

    public static readonly TaxDestinationAddress ShippingThenBilling = new("shipping_then_billing");

    public static readonly TaxDestinationAddress BillingThenShipping = new("billing_then_shipping");

    public static readonly TaxDestinationAddress ShippingOnly = new("shipping_only");

    public static readonly TaxDestinationAddress BillingOnly = new("billing_only");

    public TResult Match<TResult>(Func<TResult> onShippingThenBilling,
        Func<TResult> onBillingThenShipping,
        Func<TResult> onShippingOnly,
        Func<TResult> onBillingOnly,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == ShippingThenBilling => onShippingThenBilling(),
            _ when this == BillingThenShipping => onBillingThenShipping(),
            _ when this == ShippingOnly => onShippingOnly(),
            _ when this == BillingOnly => onBillingOnly(),
            _ => otherwise(Value)
        };

    public void Match(Action onShippingThenBilling,
        Action onBillingThenShipping,
        Action onShippingOnly,
        Action onBillingOnly,
        Action<string> otherwise)
    {
        if (this == ShippingThenBilling) onShippingThenBilling();
        else if (this == BillingThenShipping) onBillingThenShipping();
        else if (this == ShippingOnly) onShippingOnly();
        else if (this == BillingOnly) onBillingOnly();
        else otherwise(Value);
    }
}
