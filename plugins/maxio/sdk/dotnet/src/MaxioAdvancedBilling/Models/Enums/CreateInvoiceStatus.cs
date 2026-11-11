using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<CreateInvoiceStatus>))]
public sealed record CreateInvoiceStatus : OpenStringEnum<CreateInvoiceStatus>
{
    private CreateInvoiceStatus(string value) : base(value)
    {
    }

    public static readonly CreateInvoiceStatus Draft = new("draft");

    public static readonly CreateInvoiceStatus Open = new("open");

    public TResult Match<TResult>(Func<TResult> onDraft, Func<TResult> onOpen, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Draft => onDraft(),
            _ when this == Open => onOpen(),
            _ => otherwise(Value)
        };

    public void Match(Action onDraft, Action onOpen, Action<string> otherwise)
    {
        if (this == Draft) onDraft();
        else if (this == Open) onOpen();
        else otherwise(Value);
    }
}
