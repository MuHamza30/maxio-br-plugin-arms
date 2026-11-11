using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// Behavior at the hard-stop boundary. <c>discard</c> drains the wallet to zero and rejects the excess (the default). <c>allow_negative</c> deducts the full amount, letting the balance go negative. The excess past the wallet's grants is booked as an overdraft ledger row with <c>grant_id</c> set to <c>null</c>.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<WalletHardStopOverflow>))]
public sealed record WalletHardStopOverflow : OpenStringEnum<WalletHardStopOverflow>
{
    private WalletHardStopOverflow(string value) : base(value)
    {
    }

    public static readonly WalletHardStopOverflow Discard = new("discard");

    public static readonly WalletHardStopOverflow AllowNegative = new("allow_negative");

    public TResult Match<TResult>(Func<TResult> onDiscard,
        Func<TResult> onAllowNegative,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Discard => onDiscard(),
            _ when this == AllowNegative => onAllowNegative(),
            _ => otherwise(Value)
        };

    public void Match(Action onDiscard, Action onAllowNegative, Action<string> otherwise)
    {
        if (this == Discard) onDiscard();
        else if (this == AllowNegative) onAllowNegative();
        else otherwise(Value);
    }
}
