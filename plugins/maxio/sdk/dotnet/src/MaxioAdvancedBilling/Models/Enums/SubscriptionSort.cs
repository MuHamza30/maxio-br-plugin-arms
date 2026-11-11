using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<SubscriptionSort>))]
public sealed record SubscriptionSort : OpenStringEnum<SubscriptionSort>
{
    private SubscriptionSort(string value) : base(value)
    {
    }

    public static readonly SubscriptionSort SignupDate = new("signup_date");

    public static readonly SubscriptionSort PeriodStart = new("period_start");

    public static readonly SubscriptionSort PeriodEnd = new("period_end");

    public static readonly SubscriptionSort NextAssessment = new("next_assessment");

    public static readonly SubscriptionSort UpdatedAt = new("updated_at");

    public static readonly SubscriptionSort CreatedAt = new("created_at");

    public static readonly SubscriptionSort TotalPayments = new("total_payments");

    public static readonly SubscriptionSort Id = new("id");

    public static readonly SubscriptionSort OpenBalance = new("open_balance");

    public static readonly SubscriptionSort ExpiresAt = new("expires_at");

    public TResult Match<TResult>(Func<TResult> onSignupDate,
        Func<TResult> onPeriodStart,
        Func<TResult> onPeriodEnd,
        Func<TResult> onNextAssessment,
        Func<TResult> onUpdatedAt,
        Func<TResult> onCreatedAt,
        Func<TResult> onTotalPayments,
        Func<TResult> onId,
        Func<TResult> onOpenBalance,
        Func<TResult> onExpiresAt,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == SignupDate => onSignupDate(),
            _ when this == PeriodStart => onPeriodStart(),
            _ when this == PeriodEnd => onPeriodEnd(),
            _ when this == NextAssessment => onNextAssessment(),
            _ when this == UpdatedAt => onUpdatedAt(),
            _ when this == CreatedAt => onCreatedAt(),
            _ when this == TotalPayments => onTotalPayments(),
            _ when this == Id => onId(),
            _ when this == OpenBalance => onOpenBalance(),
            _ when this == ExpiresAt => onExpiresAt(),
            _ => otherwise(Value)
        };

    public void Match(Action onSignupDate,
        Action onPeriodStart,
        Action onPeriodEnd,
        Action onNextAssessment,
        Action onUpdatedAt,
        Action onCreatedAt,
        Action onTotalPayments,
        Action onId,
        Action onOpenBalance,
        Action onExpiresAt,
        Action<string> otherwise)
    {
        if (this == SignupDate) onSignupDate();
        else if (this == PeriodStart) onPeriodStart();
        else if (this == PeriodEnd) onPeriodEnd();
        else if (this == NextAssessment) onNextAssessment();
        else if (this == UpdatedAt) onUpdatedAt();
        else if (this == CreatedAt) onCreatedAt();
        else if (this == TotalPayments) onTotalPayments();
        else if (this == Id) onId();
        else if (this == OpenBalance) onOpenBalance();
        else if (this == ExpiresAt) onExpiresAt();
        else otherwise(Value);
    }
}
