using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// How the grant was created. <c>recurring</c> comes from the wallet's automatic funding schedule (see the wallet's <c>recurring_funding</c> array). <c>manual</c> comes from a one-time grant posted to <see href="$e/Wallet%20Transactions/createWalletTransaction">Create Wallet Transaction</see>. <c>initial_balance</c> comes from a non-zero <c>initial_balance</c> at wallet creation. <c>allowance_increase</c> comes from raising a funding entitlement's allowance while it's already funding a wallet. <c>purchase</c> comes from <see href="$e/Wallet%20Purchases/createWalletPurchase">Create Wallet Purchase</see>.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<Origin>))]
public sealed record Origin : OpenStringEnum<Origin>
{
    private Origin(string value) : base(value)
    {
    }

    public static readonly Origin Recurring = new("recurring");

    public static readonly Origin Manual = new("manual");

    public static readonly Origin InitialBalance = new("initial_balance");

    public static readonly Origin AllowanceIncrease = new("allowance_increase");

    public static readonly Origin Purchase = new("purchase");

    public TResult Match<TResult>(Func<TResult> onRecurring,
        Func<TResult> onManual,
        Func<TResult> onInitialBalance,
        Func<TResult> onAllowanceIncrease,
        Func<TResult> onPurchase,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Recurring => onRecurring(),
            _ when this == Manual => onManual(),
            _ when this == InitialBalance => onInitialBalance(),
            _ when this == AllowanceIncrease => onAllowanceIncrease(),
            _ when this == Purchase => onPurchase(),
            _ => otherwise(Value)
        };

    public void Match(Action onRecurring,
        Action onManual,
        Action onInitialBalance,
        Action onAllowanceIncrease,
        Action onPurchase,
        Action<string> otherwise)
    {
        if (this == Recurring) onRecurring();
        else if (this == Manual) onManual();
        else if (this == InitialBalance) onInitialBalance();
        else if (this == AllowanceIncrease) onAllowanceIncrease();
        else if (this == Purchase) onPurchase();
        else otherwise(Value);
    }
}
