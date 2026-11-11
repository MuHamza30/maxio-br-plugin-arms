using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The vault that stores the payment profile with the provided vault_token.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<ApplePayVault>))]
public sealed record ApplePayVault : OpenStringEnum<ApplePayVault>
{
    private ApplePayVault(string value) : base(value)
    {
    }

    public static readonly ApplePayVault BraintreeBlue = new("braintree_blue");

    public TResult Match<TResult>(Func<TResult> onBraintreeBlue, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == BraintreeBlue => onBraintreeBlue(),
            _ => otherwise(Value)
        };

    public void Match(Action onBraintreeBlue, Action<string> otherwise)
    {
        if (this == BraintreeBlue) onBraintreeBlue();
        else otherwise(Value);
    }
}
