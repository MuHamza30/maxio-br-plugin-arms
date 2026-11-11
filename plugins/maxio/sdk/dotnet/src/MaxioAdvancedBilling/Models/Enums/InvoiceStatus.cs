using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The current status of the invoice. See <see href="https://maxio.zendesk.com/hc/en-us/articles/24252287829645-Advanced-Billing-Invoices-Overview#invoice-statuses">Invoice Statuses</see> for more.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<InvoiceStatus>))]
public sealed record InvoiceStatus : OpenStringEnum<InvoiceStatus>
{
    private InvoiceStatus(string value) : base(value)
    {
    }

    public static readonly InvoiceStatus Draft = new("draft");

    public static readonly InvoiceStatus Open = new("open");

    public static readonly InvoiceStatus Paid = new("paid");

    public static readonly InvoiceStatus Pending = new("pending");

    public static readonly InvoiceStatus Voided = new("voided");

    public static readonly InvoiceStatus Canceled = new("canceled");

    public static readonly InvoiceStatus Processing = new("processing");

    public TResult Match<TResult>(Func<TResult> onDraft,
        Func<TResult> onOpen,
        Func<TResult> onPaid,
        Func<TResult> onPending,
        Func<TResult> onVoided,
        Func<TResult> onCanceled,
        Func<TResult> onProcessing,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Draft => onDraft(),
            _ when this == Open => onOpen(),
            _ when this == Paid => onPaid(),
            _ when this == Pending => onPending(),
            _ when this == Voided => onVoided(),
            _ when this == Canceled => onCanceled(),
            _ when this == Processing => onProcessing(),
            _ => otherwise(Value)
        };

    public void Match(Action onDraft,
        Action onOpen,
        Action onPaid,
        Action onPending,
        Action onVoided,
        Action onCanceled,
        Action onProcessing,
        Action<string> otherwise)
    {
        if (this == Draft) onDraft();
        else if (this == Open) onOpen();
        else if (this == Paid) onPaid();
        else if (this == Pending) onPending();
        else if (this == Voided) onVoided();
        else if (this == Canceled) onCanceled();
        else if (this == Processing) onProcessing();
        else otherwise(Value);
    }
}
