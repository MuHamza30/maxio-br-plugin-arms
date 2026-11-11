using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<InvoiceEventPaymentMethod>))]
public sealed record InvoiceEventPaymentMethod : OpenStringEnum<InvoiceEventPaymentMethod>
{
    private InvoiceEventPaymentMethod(string value) : base(value)
    {
    }

    public static readonly InvoiceEventPaymentMethod ApplePay = new("apple_pay");

    public static readonly InvoiceEventPaymentMethod BankAccount = new("bank_account");

    public static readonly InvoiceEventPaymentMethod CreditCard = new("credit_card");

    public static readonly InvoiceEventPaymentMethod External = new("external");

    public static readonly InvoiceEventPaymentMethod PaypalAccount = new("paypal_account");

    public TResult Match<TResult>(Func<TResult> onApplePay,
        Func<TResult> onBankAccount,
        Func<TResult> onCreditCard,
        Func<TResult> onExternal,
        Func<TResult> onPaypalAccount,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == ApplePay => onApplePay(),
            _ when this == BankAccount => onBankAccount(),
            _ when this == CreditCard => onCreditCard(),
            _ when this == External => onExternal(),
            _ when this == PaypalAccount => onPaypalAccount(),
            _ => otherwise(Value)
        };

    public void Match(Action onApplePay,
        Action onBankAccount,
        Action onCreditCard,
        Action onExternal,
        Action onPaypalAccount,
        Action<string> otherwise)
    {
        if (this == ApplePay) onApplePay();
        else if (this == BankAccount) onBankAccount();
        else if (this == CreditCard) onCreditCard();
        else if (this == External) onExternal();
        else if (this == PaypalAccount) onPaypalAccount();
        else otherwise(Value);
    }
}
