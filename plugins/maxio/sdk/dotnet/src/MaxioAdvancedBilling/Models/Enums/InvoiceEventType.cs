using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// Invoice Event Type
/// </summary>
[JsonConverter(typeof(StringEnumConverter<InvoiceEventType>))]
public sealed record InvoiceEventType : OpenStringEnum<InvoiceEventType>
{
    private InvoiceEventType(string value) : base(value)
    {
    }

    public static readonly InvoiceEventType IssueInvoice = new("issue_invoice");

    public static readonly InvoiceEventType ApplyCreditNote = new("apply_credit_note");

    public static readonly InvoiceEventType CreateCreditNote = new("create_credit_note");

    public static readonly InvoiceEventType ApplyPayment = new("apply_payment");

    public static readonly InvoiceEventType ApplyDebitNote = new("apply_debit_note");

    public static readonly InvoiceEventType CreateDebitNote = new("create_debit_note");

    public static readonly InvoiceEventType RefundInvoice = new("refund_invoice");

    public static readonly InvoiceEventType VoidInvoice = new("void_invoice");

    public static readonly InvoiceEventType VoidRemainder = new("void_remainder");

    public static readonly InvoiceEventType BackportInvoice = new("backport_invoice");

    public static readonly InvoiceEventType ChangeInvoiceStatus = new("change_invoice_status");

    public static readonly InvoiceEventType ChangeInvoiceCollectionMethod = new("change_invoice_collection_method");

    public static readonly InvoiceEventType RemovePayment = new("remove_payment");

    public static readonly InvoiceEventType FailedPayment = new("failed_payment");

    public static readonly InvoiceEventType ChangeChargebackStatus = new("change_chargeback_status");

    public TResult Match<TResult>(Func<TResult> onIssueInvoice,
        Func<TResult> onApplyCreditNote,
        Func<TResult> onCreateCreditNote,
        Func<TResult> onApplyPayment,
        Func<TResult> onApplyDebitNote,
        Func<TResult> onCreateDebitNote,
        Func<TResult> onRefundInvoice,
        Func<TResult> onVoidInvoice,
        Func<TResult> onVoidRemainder,
        Func<TResult> onBackportInvoice,
        Func<TResult> onChangeInvoiceStatus,
        Func<TResult> onChangeInvoiceCollectionMethod,
        Func<TResult> onRemovePayment,
        Func<TResult> onFailedPayment,
        Func<TResult> onChangeChargebackStatus,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == IssueInvoice => onIssueInvoice(),
            _ when this == ApplyCreditNote => onApplyCreditNote(),
            _ when this == CreateCreditNote => onCreateCreditNote(),
            _ when this == ApplyPayment => onApplyPayment(),
            _ when this == ApplyDebitNote => onApplyDebitNote(),
            _ when this == CreateDebitNote => onCreateDebitNote(),
            _ when this == RefundInvoice => onRefundInvoice(),
            _ when this == VoidInvoice => onVoidInvoice(),
            _ when this == VoidRemainder => onVoidRemainder(),
            _ when this == BackportInvoice => onBackportInvoice(),
            _ when this == ChangeInvoiceStatus => onChangeInvoiceStatus(),
            _ when this == ChangeInvoiceCollectionMethod => onChangeInvoiceCollectionMethod(),
            _ when this == RemovePayment => onRemovePayment(),
            _ when this == FailedPayment => onFailedPayment(),
            _ when this == ChangeChargebackStatus => onChangeChargebackStatus(),
            _ => otherwise(Value)
        };

    public void Match(Action onIssueInvoice,
        Action onApplyCreditNote,
        Action onCreateCreditNote,
        Action onApplyPayment,
        Action onApplyDebitNote,
        Action onCreateDebitNote,
        Action onRefundInvoice,
        Action onVoidInvoice,
        Action onVoidRemainder,
        Action onBackportInvoice,
        Action onChangeInvoiceStatus,
        Action onChangeInvoiceCollectionMethod,
        Action onRemovePayment,
        Action onFailedPayment,
        Action onChangeChargebackStatus,
        Action<string> otherwise)
    {
        if (this == IssueInvoice) onIssueInvoice();
        else if (this == ApplyCreditNote) onApplyCreditNote();
        else if (this == CreateCreditNote) onCreateCreditNote();
        else if (this == ApplyPayment) onApplyPayment();
        else if (this == ApplyDebitNote) onApplyDebitNote();
        else if (this == CreateDebitNote) onCreateDebitNote();
        else if (this == RefundInvoice) onRefundInvoice();
        else if (this == VoidInvoice) onVoidInvoice();
        else if (this == VoidRemainder) onVoidRemainder();
        else if (this == BackportInvoice) onBackportInvoice();
        else if (this == ChangeInvoiceStatus) onChangeInvoiceStatus();
        else if (this == ChangeInvoiceCollectionMethod) onChangeInvoiceCollectionMethod();
        else if (this == RemovePayment) onRemovePayment();
        else if (this == FailedPayment) onFailedPayment();
        else if (this == ChangeChargebackStatus) onChangeChargebackStatus();
        else otherwise(Value);
    }
}
