using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<SubscriptionDateField>))]
public sealed record SubscriptionDateField : OpenStringEnum<SubscriptionDateField>
{
    private SubscriptionDateField(string value) : base(value)
    {
    }

    public static readonly SubscriptionDateField CurrentPeriodEndsAt = new("current_period_ends_at");

    public static readonly SubscriptionDateField CurrentPeriodStartsAt = new("current_period_starts_at");

    public static readonly SubscriptionDateField CreatedAt = new("created_at");

    public static readonly SubscriptionDateField ActivatedAt = new("activated_at");

    public static readonly SubscriptionDateField CanceledAt = new("canceled_at");

    public static readonly SubscriptionDateField ExpiresAt = new("expires_at");

    public static readonly SubscriptionDateField TrialStartedAt = new("trial_started_at");

    public static readonly SubscriptionDateField TrialEndedAt = new("trial_ended_at");

    public static readonly SubscriptionDateField UpdatedAt = new("updated_at");

    public TResult Match<TResult>(Func<TResult> onCurrentPeriodEndsAt,
        Func<TResult> onCurrentPeriodStartsAt,
        Func<TResult> onCreatedAt,
        Func<TResult> onActivatedAt,
        Func<TResult> onCanceledAt,
        Func<TResult> onExpiresAt,
        Func<TResult> onTrialStartedAt,
        Func<TResult> onTrialEndedAt,
        Func<TResult> onUpdatedAt,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == CurrentPeriodEndsAt => onCurrentPeriodEndsAt(),
            _ when this == CurrentPeriodStartsAt => onCurrentPeriodStartsAt(),
            _ when this == CreatedAt => onCreatedAt(),
            _ when this == ActivatedAt => onActivatedAt(),
            _ when this == CanceledAt => onCanceledAt(),
            _ when this == ExpiresAt => onExpiresAt(),
            _ when this == TrialStartedAt => onTrialStartedAt(),
            _ when this == TrialEndedAt => onTrialEndedAt(),
            _ when this == UpdatedAt => onUpdatedAt(),
            _ => otherwise(Value)
        };

    public void Match(Action onCurrentPeriodEndsAt,
        Action onCurrentPeriodStartsAt,
        Action onCreatedAt,
        Action onActivatedAt,
        Action onCanceledAt,
        Action onExpiresAt,
        Action onTrialStartedAt,
        Action onTrialEndedAt,
        Action onUpdatedAt,
        Action<string> otherwise)
    {
        if (this == CurrentPeriodEndsAt) onCurrentPeriodEndsAt();
        else if (this == CurrentPeriodStartsAt) onCurrentPeriodStartsAt();
        else if (this == CreatedAt) onCreatedAt();
        else if (this == ActivatedAt) onActivatedAt();
        else if (this == CanceledAt) onCanceledAt();
        else if (this == ExpiresAt) onExpiresAt();
        else if (this == TrialStartedAt) onTrialStartedAt();
        else if (this == TrialEndedAt) onTrialEndedAt();
        else if (this == UpdatedAt) onUpdatedAt();
        else otherwise(Value);
    }
}
