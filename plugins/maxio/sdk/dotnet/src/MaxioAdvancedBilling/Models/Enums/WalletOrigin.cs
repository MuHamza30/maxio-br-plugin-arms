using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// How the wallet came to exist. <c>manual</c> for a wallet created directly with <see href="$e/Wallets/createWallet">Create Wallet</see>. <c>feature</c> for a wallet auto-provisioned because a subscription resolved a usage-limit entitlement whose Feature has <c>wallet_behavior</c> configured.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<WalletOrigin>))]
public sealed record WalletOrigin : OpenStringEnum<WalletOrigin>
{
    private WalletOrigin(string value) : base(value)
    {
    }

    public static readonly WalletOrigin Manual = new("manual");

    public static readonly WalletOrigin Feature = new("feature");

    public TResult Match<TResult>(Func<TResult> onManual, Func<TResult> onFeature, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Manual => onManual(),
            _ when this == Feature => onFeature(),
            _ => otherwise(Value)
        };

    public void Match(Action onManual, Action onFeature, Action<string> otherwise)
    {
        if (this == Manual) onManual();
        else if (this == Feature) onFeature();
        else otherwise(Value);
    }
}
