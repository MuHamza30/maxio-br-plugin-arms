using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<CreateSignupProformaPreviewInclude>))]
public sealed record CreateSignupProformaPreviewInclude : OpenStringEnum<CreateSignupProformaPreviewInclude>
{
    private CreateSignupProformaPreviewInclude(string value) : base(value)
    {
    }

    public static readonly CreateSignupProformaPreviewInclude NextProformaInvoice = new("next_proforma_invoice");

    public TResult Match<TResult>(Func<TResult> onNextProformaInvoice, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == NextProformaInvoice => onNextProformaInvoice(),
            _ => otherwise(Value)
        };

    public void Match(Action onNextProformaInvoice, Action<string> otherwise)
    {
        if (this == NextProformaInvoice) onNextProformaInvoice();
        else otherwise(Value);
    }
}
