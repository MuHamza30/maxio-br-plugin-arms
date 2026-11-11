using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The type of entry
/// </summary>
[JsonConverter(typeof(StringEnumConverter<ServiceCreditType>))]
public sealed record ServiceCreditType : OpenStringEnum<ServiceCreditType>
{
    private ServiceCreditType(string value) : base(value)
    {
    }

    public static readonly ServiceCreditType Credit = new("Credit");

    public static readonly ServiceCreditType Debit = new("Debit");

    public TResult Match<TResult>(Func<TResult> onCredit, Func<TResult> onDebit, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Credit => onCredit(),
            _ when this == Debit => onDebit(),
            _ => otherwise(Value)
        };

    public void Match(Action onCredit, Action onDebit, Action<string> otherwise)
    {
        if (this == Credit) onCredit();
        else if (this == Debit) onDebit();
        else otherwise(Value);
    }
}
