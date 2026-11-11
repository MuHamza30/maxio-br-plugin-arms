using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// 'proforma' value is deprecated in favor of proforma_adhoc and proforma_automatic.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<ProformaInvoiceRole>))]
public sealed record ProformaInvoiceRole : OpenStringEnum<ProformaInvoiceRole>
{
    private ProformaInvoiceRole(string value) : base(value)
    {
    }

    public static readonly ProformaInvoiceRole Unset = new("unset");

    public static readonly ProformaInvoiceRole Proforma = new("proforma");

    public static readonly ProformaInvoiceRole ProformaAdhoc = new("proforma_adhoc");

    public static readonly ProformaInvoiceRole ProformaAutomatic = new("proforma_automatic");

    public TResult Match<TResult>(Func<TResult> onUnset,
        Func<TResult> onProforma,
        Func<TResult> onProformaAdhoc,
        Func<TResult> onProformaAutomatic,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Unset => onUnset(),
            _ when this == Proforma => onProforma(),
            _ when this == ProformaAdhoc => onProformaAdhoc(),
            _ when this == ProformaAutomatic => onProformaAutomatic(),
            _ => otherwise(Value)
        };

    public void Match(Action onUnset,
        Action onProforma,
        Action onProformaAdhoc,
        Action onProformaAutomatic,
        Action<string> otherwise)
    {
        if (this == Unset) onUnset();
        else if (this == Proforma) onProforma();
        else if (this == ProformaAdhoc) onProformaAdhoc();
        else if (this == ProformaAutomatic) onProformaAutomatic();
        else otherwise(Value);
    }
}
