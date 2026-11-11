using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// A handle for the line item transaction type
/// </summary>
[JsonConverter(typeof(StringEnumConverter<LineItemTransactionType>))]
public sealed record LineItemTransactionType : OpenStringEnum<LineItemTransactionType>
{
    private LineItemTransactionType(string value) : base(value)
    {
    }

    public static readonly LineItemTransactionType Charge = new("charge");

    public static readonly LineItemTransactionType Credit = new("credit");

    public static readonly LineItemTransactionType Adjustment = new("adjustment");

    public static readonly LineItemTransactionType Payment = new("payment");

    public static readonly LineItemTransactionType Refund = new("refund");

    public static readonly LineItemTransactionType InfoTransaction = new("info_transaction");

    public static readonly LineItemTransactionType PaymentAuthorization = new("payment_authorization");

    public TResult Match<TResult>(Func<TResult> onCharge,
        Func<TResult> onCredit,
        Func<TResult> onAdjustment,
        Func<TResult> onPayment,
        Func<TResult> onRefund,
        Func<TResult> onInfoTransaction,
        Func<TResult> onPaymentAuthorization,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Charge => onCharge(),
            _ when this == Credit => onCredit(),
            _ when this == Adjustment => onAdjustment(),
            _ when this == Payment => onPayment(),
            _ when this == Refund => onRefund(),
            _ when this == InfoTransaction => onInfoTransaction(),
            _ when this == PaymentAuthorization => onPaymentAuthorization(),
            _ => otherwise(Value)
        };

    public void Match(Action onCharge,
        Action onCredit,
        Action onAdjustment,
        Action onPayment,
        Action onRefund,
        Action onInfoTransaction,
        Action onPaymentAuthorization,
        Action<string> otherwise)
    {
        if (this == Charge) onCharge();
        else if (this == Credit) onCredit();
        else if (this == Adjustment) onAdjustment();
        else if (this == Payment) onPayment();
        else if (this == Refund) onRefund();
        else if (this == InfoTransaction) onInfoTransaction();
        else if (this == PaymentAuthorization) onPaymentAuthorization();
        else otherwise(Value);
    }
}
