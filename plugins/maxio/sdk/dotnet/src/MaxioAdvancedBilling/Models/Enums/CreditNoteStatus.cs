using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// Current status of the credit note.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<CreditNoteStatus>))]
public sealed record CreditNoteStatus : OpenStringEnum<CreditNoteStatus>
{
    private CreditNoteStatus(string value) : base(value)
    {
    }

    public static readonly CreditNoteStatus Open = new("open");

    public static readonly CreditNoteStatus Applied = new("applied");

    public TResult Match<TResult>(Func<TResult> onOpen, Func<TResult> onApplied, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Open => onOpen(),
            _ when this == Applied => onApplied(),
            _ => otherwise(Value)
        };

    public void Match(Action onOpen, Action onApplied, Action<string> otherwise)
    {
        if (this == Open) onOpen();
        else if (this == Applied) onApplied();
        else otherwise(Value);
    }
}
