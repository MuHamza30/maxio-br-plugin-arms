using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<PaymentType>))]
public sealed record PaymentType : OpenStringEnum<PaymentType>
{
    private PaymentType(string value) : base(value)
    {
    }

    public static readonly PaymentType CreditCard = new("credit_card");

    public static readonly PaymentType BankAccount = new("bank_account");

    public static readonly PaymentType PaypalAccount = new("paypal_account");

    public static readonly PaymentType ApplePay = new("apple_pay");

    public TResult Match<TResult>(Func<TResult> onCreditCard,
        Func<TResult> onBankAccount,
        Func<TResult> onPaypalAccount,
        Func<TResult> onApplePay,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == CreditCard => onCreditCard(),
            _ when this == BankAccount => onBankAccount(),
            _ when this == PaypalAccount => onPaypalAccount(),
            _ when this == ApplePay => onApplePay(),
            _ => otherwise(Value)
        };

    public void Match(Action onCreditCard,
        Action onBankAccount,
        Action onPaypalAccount,
        Action onApplePay,
        Action<string> otherwise)
    {
        if (this == CreditCard) onCreditCard();
        else if (this == BankAccount) onBankAccount();
        else if (this == PaypalAccount) onPaypalAccount();
        else if (this == ApplePay) onApplePay();
        else otherwise(Value);
    }
}
