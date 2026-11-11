using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The vault that stores the payment profile with the provided vault_token.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<PayPalVault>))]
public sealed record PayPalVault : OpenStringEnum<PayPalVault>
{
    private PayPalVault(string value) : base(value)
    {
    }

    public static readonly PayPalVault BraintreeBlue = new("braintree_blue");

    public static readonly PayPalVault Paypal = new("paypal");

    public static readonly PayPalVault Moduslink = new("moduslink");

    public static readonly PayPalVault PaypalComplete = new("paypal_complete");

    public TResult Match<TResult>(Func<TResult> onBraintreeBlue,
        Func<TResult> onPaypal,
        Func<TResult> onModuslink,
        Func<TResult> onPaypalComplete,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == BraintreeBlue => onBraintreeBlue(),
            _ when this == Paypal => onPaypal(),
            _ when this == Moduslink => onModuslink(),
            _ when this == PaypalComplete => onPaypalComplete(),
            _ => otherwise(Value)
        };

    public void Match(Action onBraintreeBlue,
        Action onPaypal,
        Action onModuslink,
        Action onPaypalComplete,
        Action<string> otherwise)
    {
        if (this == BraintreeBlue) onBraintreeBlue();
        else if (this == Paypal) onPaypal();
        else if (this == Moduslink) onModuslink();
        else if (this == PaypalComplete) onPaypalComplete();
        else otherwise(Value);
    }
}
