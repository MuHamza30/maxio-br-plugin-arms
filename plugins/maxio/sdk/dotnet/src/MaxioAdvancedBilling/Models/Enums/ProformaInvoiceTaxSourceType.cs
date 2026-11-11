using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<ProformaInvoiceTaxSourceType>))]
public sealed record ProformaInvoiceTaxSourceType : OpenStringEnum<ProformaInvoiceTaxSourceType>
{
    private ProformaInvoiceTaxSourceType(string value) : base(value)
    {
    }

    public static readonly ProformaInvoiceTaxSourceType Tax = new("Tax");

    public static readonly ProformaInvoiceTaxSourceType Avalara = new("Avalara");

    public TResult Match<TResult>(Func<TResult> onTax, Func<TResult> onAvalara, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Tax => onTax(),
            _ when this == Avalara => onAvalara(),
            _ => otherwise(Value)
        };

    public void Match(Action onTax, Action onAvalara, Action<string> otherwise)
    {
        if (this == Tax) onTax();
        else if (this == Avalara) onAvalara();
        else otherwise(Value);
    }
}
