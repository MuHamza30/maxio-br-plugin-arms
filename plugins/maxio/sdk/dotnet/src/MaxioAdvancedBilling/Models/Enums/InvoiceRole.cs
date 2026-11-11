using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<InvoiceRole>))]
public sealed record InvoiceRole : OpenStringEnum<InvoiceRole>
{
    private InvoiceRole(string value) : base(value)
    {
    }

    public static readonly InvoiceRole Unset = new("unset");

    public static readonly InvoiceRole Signup = new("signup");

    public static readonly InvoiceRole Renewal = new("renewal");

    public static readonly InvoiceRole Usage = new("usage");

    public static readonly InvoiceRole Reactivation = new("reactivation");

    public static readonly InvoiceRole Proration = new("proration");

    public static readonly InvoiceRole Migration = new("migration");

    public static readonly InvoiceRole Adhoc = new("adhoc");

    public static readonly InvoiceRole Backport = new("backport");

    public static readonly InvoiceRole BackportBalanceReconciliation = new("backport-balance-reconciliation");

    public TResult Match<TResult>(Func<TResult> onUnset,
        Func<TResult> onSignup,
        Func<TResult> onRenewal,
        Func<TResult> onUsage,
        Func<TResult> onReactivation,
        Func<TResult> onProration,
        Func<TResult> onMigration,
        Func<TResult> onAdhoc,
        Func<TResult> onBackport,
        Func<TResult> onBackportBalanceReconciliation,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Unset => onUnset(),
            _ when this == Signup => onSignup(),
            _ when this == Renewal => onRenewal(),
            _ when this == Usage => onUsage(),
            _ when this == Reactivation => onReactivation(),
            _ when this == Proration => onProration(),
            _ when this == Migration => onMigration(),
            _ when this == Adhoc => onAdhoc(),
            _ when this == Backport => onBackport(),
            _ when this == BackportBalanceReconciliation => onBackportBalanceReconciliation(),
            _ => otherwise(Value)
        };

    public void Match(Action onUnset,
        Action onSignup,
        Action onRenewal,
        Action onUsage,
        Action onReactivation,
        Action onProration,
        Action onMigration,
        Action onAdhoc,
        Action onBackport,
        Action onBackportBalanceReconciliation,
        Action<string> otherwise)
    {
        if (this == Unset) onUnset();
        else if (this == Signup) onSignup();
        else if (this == Renewal) onRenewal();
        else if (this == Usage) onUsage();
        else if (this == Reactivation) onReactivation();
        else if (this == Proration) onProration();
        else if (this == Migration) onMigration();
        else if (this == Adhoc) onAdhoc();
        else if (this == Backport) onBackport();
        else if (this == BackportBalanceReconciliation) onBackportBalanceReconciliation();
        else otherwise(Value);
    }
}
