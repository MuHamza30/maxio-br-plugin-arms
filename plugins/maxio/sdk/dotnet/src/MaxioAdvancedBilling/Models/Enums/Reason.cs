using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// Set whenever <c>status</c> isn't <c>active</c>. Accompanying <c>stopped</c>: <c>entitlement_missing</c> (an ordinary product or component change removed the entitlement), <c>wallet_archived</c> (the wallet was archived), <c>subscription_terminated</c> (the subscription reached a terminal state), <c>feature_removed</c> (the subscription no longer has this Feature), <c>wallet_behavior_disabled</c> (the Feature no longer has <c>wallet_behavior</c> configured), <c>superseded_by_replacement</c> (a later entitlement now funds this wallet instead), or <c>subscription_prepaid</c> (the subscription switched to prepaid, which cannot carry an active wallet). Accompanying <c>erroring</c>: <c>allowance_not_representable</c> (the entitlement's current allowance doesn't fit the wallet's amount encoding) or <c>balance_at_capacity</c> (granting it would push the balance past the maximum supported value).
/// </summary>
[JsonConverter(typeof(StringEnumConverter<Reason>))]
public sealed record Reason : OpenStringEnum<Reason>
{
    private Reason(string value) : base(value)
    {
    }

    public static readonly Reason EntitlementMissing = new("entitlement_missing");

    public static readonly Reason WalletArchived = new("wallet_archived");

    public static readonly Reason SubscriptionTerminated = new("subscription_terminated");

    public static readonly Reason FeatureRemoved = new("feature_removed");

    public static readonly Reason WalletBehaviorDisabled = new("wallet_behavior_disabled");

    public static readonly Reason SupersededByReplacement = new("superseded_by_replacement");

    public static readonly Reason SubscriptionPrepaid = new("subscription_prepaid");

    public static readonly Reason AllowanceNotRepresentable = new("allowance_not_representable");

    public static readonly Reason BalanceAtCapacity = new("balance_at_capacity");

    public TResult Match<TResult>(Func<TResult> onEntitlementMissing,
        Func<TResult> onWalletArchived,
        Func<TResult> onSubscriptionTerminated,
        Func<TResult> onFeatureRemoved,
        Func<TResult> onWalletBehaviorDisabled,
        Func<TResult> onSupersededByReplacement,
        Func<TResult> onSubscriptionPrepaid,
        Func<TResult> onAllowanceNotRepresentable,
        Func<TResult> onBalanceAtCapacity,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == EntitlementMissing => onEntitlementMissing(),
            _ when this == WalletArchived => onWalletArchived(),
            _ when this == SubscriptionTerminated => onSubscriptionTerminated(),
            _ when this == FeatureRemoved => onFeatureRemoved(),
            _ when this == WalletBehaviorDisabled => onWalletBehaviorDisabled(),
            _ when this == SupersededByReplacement => onSupersededByReplacement(),
            _ when this == SubscriptionPrepaid => onSubscriptionPrepaid(),
            _ when this == AllowanceNotRepresentable => onAllowanceNotRepresentable(),
            _ when this == BalanceAtCapacity => onBalanceAtCapacity(),
            _ => otherwise(Value)
        };

    public void Match(Action onEntitlementMissing,
        Action onWalletArchived,
        Action onSubscriptionTerminated,
        Action onFeatureRemoved,
        Action onWalletBehaviorDisabled,
        Action onSupersededByReplacement,
        Action onSubscriptionPrepaid,
        Action onAllowanceNotRepresentable,
        Action onBalanceAtCapacity,
        Action<string> otherwise)
    {
        if (this == EntitlementMissing) onEntitlementMissing();
        else if (this == WalletArchived) onWalletArchived();
        else if (this == SubscriptionTerminated) onSubscriptionTerminated();
        else if (this == FeatureRemoved) onFeatureRemoved();
        else if (this == WalletBehaviorDisabled) onWalletBehaviorDisabled();
        else if (this == SupersededByReplacement) onSupersededByReplacement();
        else if (this == SubscriptionPrepaid) onSubscriptionPrepaid();
        else if (this == AllowanceNotRepresentable) onAllowanceNotRepresentable();
        else if (this == BalanceAtCapacity) onBalanceAtCapacity();
        else otherwise(Value);
    }
}
