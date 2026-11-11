using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// Allowed values for filtering by the current state of the subscription.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<SubscriptionStateFilter>))]
public sealed record SubscriptionStateFilter : OpenStringEnum<SubscriptionStateFilter>
{
    private SubscriptionStateFilter(string value) : base(value)
    {
    }

    public static readonly SubscriptionStateFilter Active = new("active");

    public static readonly SubscriptionStateFilter Canceled = new("canceled");

    public static readonly SubscriptionStateFilter Expired = new("expired");

    public static readonly SubscriptionStateFilter ExpiredCards = new("expired_cards");

    public static readonly SubscriptionStateFilter ExpiredCardsLiveSubscriptions = new(
        "expired_cards_(live_subscriptions)");

    public static readonly SubscriptionStateFilter ExpiredCardsAllSubscriptions = new(
        "expired_cards_(all_subscriptions)");

    public static readonly SubscriptionStateFilter OnHold = new("on_hold");

    public static readonly SubscriptionStateFilter AwaitingSignup = new("awaiting_signup");

    public static readonly SubscriptionStateFilter AwaitingSignupDate = new("awaiting_signup_date");

    public static readonly SubscriptionStateFilter PastDue = new("past_due");

    public static readonly SubscriptionStateFilter PendingCancellation = new("pending_cancellation");

    public static readonly SubscriptionStateFilter PendingRenewal = new("pending_renewal");

    public static readonly SubscriptionStateFilter PrepaidDunning = new("prepaid_dunning");

    public static readonly SubscriptionStateFilter Suspended = new("suspended");

    public static readonly SubscriptionStateFilter TrialEnded = new("trial_ended");

    public static readonly SubscriptionStateFilter Trialing = new("trialing");

    public static readonly SubscriptionStateFilter Unpaid = new("unpaid");

    public TResult Match<TResult>(Func<TResult> onActive,
        Func<TResult> onCanceled,
        Func<TResult> onExpired,
        Func<TResult> onExpiredCards,
        Func<TResult> onExpiredCardsLiveSubscriptions,
        Func<TResult> onExpiredCardsAllSubscriptions,
        Func<TResult> onOnHold,
        Func<TResult> onAwaitingSignup,
        Func<TResult> onAwaitingSignupDate,
        Func<TResult> onPastDue,
        Func<TResult> onPendingCancellation,
        Func<TResult> onPendingRenewal,
        Func<TResult> onPrepaidDunning,
        Func<TResult> onSuspended,
        Func<TResult> onTrialEnded,
        Func<TResult> onTrialing,
        Func<TResult> onUnpaid,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Active => onActive(),
            _ when this == Canceled => onCanceled(),
            _ when this == Expired => onExpired(),
            _ when this == ExpiredCards => onExpiredCards(),
            _ when this == ExpiredCardsLiveSubscriptions => onExpiredCardsLiveSubscriptions(),
            _ when this == ExpiredCardsAllSubscriptions => onExpiredCardsAllSubscriptions(),
            _ when this == OnHold => onOnHold(),
            _ when this == AwaitingSignup => onAwaitingSignup(),
            _ when this == AwaitingSignupDate => onAwaitingSignupDate(),
            _ when this == PastDue => onPastDue(),
            _ when this == PendingCancellation => onPendingCancellation(),
            _ when this == PendingRenewal => onPendingRenewal(),
            _ when this == PrepaidDunning => onPrepaidDunning(),
            _ when this == Suspended => onSuspended(),
            _ when this == TrialEnded => onTrialEnded(),
            _ when this == Trialing => onTrialing(),
            _ when this == Unpaid => onUnpaid(),
            _ => otherwise(Value)
        };

    public void Match(Action onActive,
        Action onCanceled,
        Action onExpired,
        Action onExpiredCards,
        Action onExpiredCardsLiveSubscriptions,
        Action onExpiredCardsAllSubscriptions,
        Action onOnHold,
        Action onAwaitingSignup,
        Action onAwaitingSignupDate,
        Action onPastDue,
        Action onPendingCancellation,
        Action onPendingRenewal,
        Action onPrepaidDunning,
        Action onSuspended,
        Action onTrialEnded,
        Action onTrialing,
        Action onUnpaid,
        Action<string> otherwise)
    {
        if (this == Active) onActive();
        else if (this == Canceled) onCanceled();
        else if (this == Expired) onExpired();
        else if (this == ExpiredCards) onExpiredCards();
        else if (this == ExpiredCardsLiveSubscriptions) onExpiredCardsLiveSubscriptions();
        else if (this == ExpiredCardsAllSubscriptions) onExpiredCardsAllSubscriptions();
        else if (this == OnHold) onOnHold();
        else if (this == AwaitingSignup) onAwaitingSignup();
        else if (this == AwaitingSignupDate) onAwaitingSignupDate();
        else if (this == PastDue) onPastDue();
        else if (this == PendingCancellation) onPendingCancellation();
        else if (this == PendingRenewal) onPendingRenewal();
        else if (this == PrepaidDunning) onPrepaidDunning();
        else if (this == Suspended) onSuspended();
        else if (this == TrialEnded) onTrialEnded();
        else if (this == Trialing) onTrialing();
        else if (this == Unpaid) onUnpaid();
        else otherwise(Value);
    }
}
