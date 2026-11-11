using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// When the <c>method</c> specified is <c>"credit_card_on_file"</c>, the prepayment amount will be collected using the default credit card payment profile and applied to the prepayment account balance. This is especially useful for manual replenishment of prepaid subscriptions.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<CreatePrepaymentMethod>))]
public sealed record CreatePrepaymentMethod : OpenStringEnum<CreatePrepaymentMethod>
{
    private CreatePrepaymentMethod(string value) : base(value)
    {
    }

    public static readonly CreatePrepaymentMethod Check = new("check");

    public static readonly CreatePrepaymentMethod Cash = new("cash");

    public static readonly CreatePrepaymentMethod MoneyOrder = new("money_order");

    public static readonly CreatePrepaymentMethod Ach = new("ach");

    public static readonly CreatePrepaymentMethod PaypalAccount = new("paypal_account");

    public static readonly CreatePrepaymentMethod CreditCard = new("credit_card");

    public static readonly CreatePrepaymentMethod CreditCardOnFile = new("credit_card_on_file");

    public static readonly CreatePrepaymentMethod Other = new("other");

    public TResult Match<TResult>(Func<TResult> onCheck,
        Func<TResult> onCash,
        Func<TResult> onMoneyOrder,
        Func<TResult> onAch,
        Func<TResult> onPaypalAccount,
        Func<TResult> onCreditCard,
        Func<TResult> onCreditCardOnFile,
        Func<TResult> onOther,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Check => onCheck(),
            _ when this == Cash => onCash(),
            _ when this == MoneyOrder => onMoneyOrder(),
            _ when this == Ach => onAch(),
            _ when this == PaypalAccount => onPaypalAccount(),
            _ when this == CreditCard => onCreditCard(),
            _ when this == CreditCardOnFile => onCreditCardOnFile(),
            _ when this == Other => onOther(),
            _ => otherwise(Value)
        };

    public void Match(Action onCheck,
        Action onCash,
        Action onMoneyOrder,
        Action onAch,
        Action onPaypalAccount,
        Action onCreditCard,
        Action onCreditCardOnFile,
        Action onOther,
        Action<string> otherwise)
    {
        if (this == Check) onCheck();
        else if (this == Cash) onCash();
        else if (this == MoneyOrder) onMoneyOrder();
        else if (this == Ach) onAch();
        else if (this == PaypalAccount) onPaypalAccount();
        else if (this == CreditCard) onCreditCard();
        else if (this == CreditCardOnFile) onCreditCardOnFile();
        else if (this == Other) onOther();
        else otherwise(Value);
    }
}
