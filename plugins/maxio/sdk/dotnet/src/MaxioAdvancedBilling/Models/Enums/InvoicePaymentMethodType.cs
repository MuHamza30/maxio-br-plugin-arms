using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The type of payment method used. Defaults to other.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<InvoicePaymentMethodType>))]
public sealed record InvoicePaymentMethodType : OpenStringEnum<InvoicePaymentMethodType>
{
    private InvoicePaymentMethodType(string value) : base(value)
    {
    }

    public static readonly InvoicePaymentMethodType CreditCard = new("credit_card");

    public static readonly InvoicePaymentMethodType Check = new("check");

    public static readonly InvoicePaymentMethodType Cash = new("cash");

    public static readonly InvoicePaymentMethodType MoneyOrder = new("money_order");

    public static readonly InvoicePaymentMethodType Ach = new("ach");

    public static readonly InvoicePaymentMethodType Other = new("other");

    public TResult Match<TResult>(Func<TResult> onCreditCard,
        Func<TResult> onCheck,
        Func<TResult> onCash,
        Func<TResult> onMoneyOrder,
        Func<TResult> onAch,
        Func<TResult> onOther,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == CreditCard => onCreditCard(),
            _ when this == Check => onCheck(),
            _ when this == Cash => onCash(),
            _ when this == MoneyOrder => onMoneyOrder(),
            _ when this == Ach => onAch(),
            _ when this == Other => onOther(),
            _ => otherwise(Value)
        };

    public void Match(Action onCreditCard,
        Action onCheck,
        Action onCash,
        Action onMoneyOrder,
        Action onAch,
        Action onOther,
        Action<string> otherwise)
    {
        if (this == CreditCard) onCreditCard();
        else if (this == Check) onCheck();
        else if (this == Cash) onCash();
        else if (this == MoneyOrder) onMoneyOrder();
        else if (this == Ach) onAch();
        else if (this == Other) onOther();
        else otherwise(Value);
    }
}
