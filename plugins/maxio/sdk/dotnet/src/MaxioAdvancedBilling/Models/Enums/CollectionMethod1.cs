using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<CollectionMethod1>))]
public sealed record CollectionMethod1 : OpenStringEnum<CollectionMethod1>
{
    private CollectionMethod1(string value) : base(value)
    {
    }

    public static readonly CollectionMethod1 Automatic = new("automatic");

    public static readonly CollectionMethod1 Remittance = new("remittance");

    public static readonly CollectionMethod1 Prepaid = new("prepaid");

    public TResult Match<TResult>(Func<TResult> onAutomatic,
        Func<TResult> onRemittance,
        Func<TResult> onPrepaid,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Automatic => onAutomatic(),
            _ when this == Remittance => onRemittance(),
            _ when this == Prepaid => onPrepaid(),
            _ => otherwise(Value)
        };

    public void Match(Action onAutomatic, Action onRemittance, Action onPrepaid, Action<string> otherwise)
    {
        if (this == Automatic) onAutomatic();
        else if (this == Remittance) onRemittance();
        else if (this == Prepaid) onPrepaid();
        else otherwise(Value);
    }
}
