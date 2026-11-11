using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<PrepaymentMethod>))]
public sealed record PrepaymentMethod : OpenStringEnum<PrepaymentMethod>
{
    private PrepaymentMethod(string value) : base(value)
    {
    }

    public static readonly PrepaymentMethod Check = new("check");

    public static readonly PrepaymentMethod Cash = new("cash");

    public static readonly PrepaymentMethod MoneyOrder = new("money_order");

    public static readonly PrepaymentMethod Ach = new("ach");

    public static readonly PrepaymentMethod PaypalAccount = new("paypal_account");

    public static readonly PrepaymentMethod CreditCard = new("credit_card");

    public static readonly PrepaymentMethod Other = new("other");

    public TResult Match<TResult>(Func<TResult> onCheck,
        Func<TResult> onCash,
        Func<TResult> onMoneyOrder,
        Func<TResult> onAch,
        Func<TResult> onPaypalAccount,
        Func<TResult> onCreditCard,
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
            _ when this == Other => onOther(),
            _ => otherwise(Value)
        };

    public void Match(Action onCheck,
        Action onCash,
        Action onMoneyOrder,
        Action onAch,
        Action onPaypalAccount,
        Action onCreditCard,
        Action onOther,
        Action<string> otherwise)
    {
        if (this == Check) onCheck();
        else if (this == Cash) onCash();
        else if (this == MoneyOrder) onMoneyOrder();
        else if (this == Ach) onAch();
        else if (this == PaypalAccount) onPaypalAccount();
        else if (this == CreditCard) onCreditCard();
        else if (this == Other) onOther();
        else otherwise(Value);
    }
}
