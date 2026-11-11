using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The type of payment to be applied to an Invoice. Defaults to external.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<InvoicePaymentType>))]
public sealed record InvoicePaymentType : OpenStringEnum<InvoicePaymentType>
{
    private InvoicePaymentType(string value) : base(value)
    {
    }

    public static readonly InvoicePaymentType External = new("external");

    public static readonly InvoicePaymentType Prepayment = new("prepayment");

    public static readonly InvoicePaymentType ServiceCredit = new("service_credit");

    public static readonly InvoicePaymentType Payment = new("payment");

    public TResult Match<TResult>(Func<TResult> onExternal,
        Func<TResult> onPrepayment,
        Func<TResult> onServiceCredit,
        Func<TResult> onPayment,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == External => onExternal(),
            _ when this == Prepayment => onPrepayment(),
            _ when this == ServiceCredit => onServiceCredit(),
            _ when this == Payment => onPayment(),
            _ => otherwise(Value)
        };

    public void Match(Action onExternal,
        Action onPrepayment,
        Action onServiceCredit,
        Action onPayment,
        Action<string> otherwise)
    {
        if (this == External) onExternal();
        else if (this == Prepayment) onPrepayment();
        else if (this == ServiceCredit) onServiceCredit();
        else if (this == Payment) onPayment();
        else otherwise(Value);
    }
}
