using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The type of payment collection to be used in the subscription. For legacy Statements Architecture valid options are - <c>invoice</c>, <c>automatic</c>. For current Relationship Invoicing Architecture valid options are - <c>remittance</c>, <c>automatic</c>, <c>prepaid</c>.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<CollectionMethod>))]
public sealed record CollectionMethod : OpenStringEnum<CollectionMethod>
{
    private CollectionMethod(string value) : base(value)
    {
    }

    public static readonly CollectionMethod Automatic = new("automatic");

    public static readonly CollectionMethod Remittance = new("remittance");

    public static readonly CollectionMethod Prepaid = new("prepaid");

    public static readonly CollectionMethod Invoice = new("invoice");

    public TResult Match<TResult>(Func<TResult> onAutomatic,
        Func<TResult> onRemittance,
        Func<TResult> onPrepaid,
        Func<TResult> onInvoice,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Automatic => onAutomatic(),
            _ when this == Remittance => onRemittance(),
            _ when this == Prepaid => onPrepaid(),
            _ when this == Invoice => onInvoice(),
            _ => otherwise(Value)
        };

    public void Match(Action onAutomatic,
        Action onRemittance,
        Action onPrepaid,
        Action onInvoice,
        Action<string> otherwise)
    {
        if (this == Automatic) onAutomatic();
        else if (this == Remittance) onRemittance();
        else if (this == Prepaid) onPrepaid();
        else if (this == Invoice) onInvoice();
        else otherwise(Value);
    }
}
