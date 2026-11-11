using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<CreditScheme>))]
public sealed record CreditScheme : OpenStringEnum<CreditScheme>
{
    private CreditScheme(string value) : base(value)
    {
    }

    public static readonly CreditScheme None = new("none");

    public static readonly CreditScheme Credit = new("credit");

    public static readonly CreditScheme Refund = new("refund");

    public TResult Match<TResult>(Func<TResult> onNone,
        Func<TResult> onCredit,
        Func<TResult> onRefund,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == None => onNone(),
            _ when this == Credit => onCredit(),
            _ when this == Refund => onRefund(),
            _ => otherwise(Value)
        };

    public void Match(Action onNone, Action onCredit, Action onRefund, Action<string> otherwise)
    {
        if (this == None) onNone();
        else if (this == Credit) onCredit();
        else if (this == Refund) onRefund();
        else otherwise(Value);
    }
}
