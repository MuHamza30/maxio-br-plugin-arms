using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<TaxConfigurationKind>))]
public sealed record TaxConfigurationKind : OpenStringEnum<TaxConfigurationKind>
{
    private TaxConfigurationKind(string value) : base(value)
    {
    }

    public static readonly TaxConfigurationKind Custom = new("custom");

    public static readonly TaxConfigurationKind ManagedAvalara = new("managed avalara");

    public static readonly TaxConfigurationKind LinkedAvalara = new("linked avalara");

    public static readonly TaxConfigurationKind DigitalRiver = new("digital river");

    public TResult Match<TResult>(Func<TResult> onCustom,
        Func<TResult> onManagedAvalara,
        Func<TResult> onLinkedAvalara,
        Func<TResult> onDigitalRiver,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Custom => onCustom(),
            _ when this == ManagedAvalara => onManagedAvalara(),
            _ when this == LinkedAvalara => onLinkedAvalara(),
            _ when this == DigitalRiver => onDigitalRiver(),
            _ => otherwise(Value)
        };

    public void Match(Action onCustom,
        Action onManagedAvalara,
        Action onLinkedAvalara,
        Action onDigitalRiver,
        Action<string> otherwise)
    {
        if (this == Custom) onCustom();
        else if (this == ManagedAvalara) onManagedAvalara();
        else if (this == LinkedAvalara) onLinkedAvalara();
        else if (this == DigitalRiver) onDigitalRiver();
        else otherwise(Value);
    }
}
