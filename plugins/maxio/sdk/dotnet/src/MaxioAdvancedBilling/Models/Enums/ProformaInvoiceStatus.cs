using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<ProformaInvoiceStatus>))]
public sealed record ProformaInvoiceStatus : OpenStringEnum<ProformaInvoiceStatus>
{
    private ProformaInvoiceStatus(string value) : base(value)
    {
    }

    public static readonly ProformaInvoiceStatus Draft = new("draft");

    public static readonly ProformaInvoiceStatus Voided = new("voided");

    public static readonly ProformaInvoiceStatus Archived = new("archived");

    public TResult Match<TResult>(Func<TResult> onDraft,
        Func<TResult> onVoided,
        Func<TResult> onArchived,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Draft => onDraft(),
            _ when this == Voided => onVoided(),
            _ when this == Archived => onArchived(),
            _ => otherwise(Value)
        };

    public void Match(Action onDraft, Action onVoided, Action onArchived, Action<string> otherwise)
    {
        if (this == Draft) onDraft();
        else if (this == Voided) onVoided();
        else if (this == Archived) onArchived();
        else otherwise(Value);
    }
}
