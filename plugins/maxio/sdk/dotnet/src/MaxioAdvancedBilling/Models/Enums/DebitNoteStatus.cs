using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// Current status of the debit note.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<DebitNoteStatus>))]
public sealed record DebitNoteStatus : OpenStringEnum<DebitNoteStatus>
{
    private DebitNoteStatus(string value) : base(value)
    {
    }

    public static readonly DebitNoteStatus Open = new("open");

    public static readonly DebitNoteStatus Applied = new("applied");

    public static readonly DebitNoteStatus Banished = new("banished");

    public static readonly DebitNoteStatus Paid = new("paid");

    public TResult Match<TResult>(Func<TResult> onOpen,
        Func<TResult> onApplied,
        Func<TResult> onBanished,
        Func<TResult> onPaid,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Open => onOpen(),
            _ when this == Applied => onApplied(),
            _ when this == Banished => onBanished(),
            _ when this == Paid => onPaid(),
            _ => otherwise(Value)
        };

    public void Match(Action onOpen, Action onApplied, Action onBanished, Action onPaid, Action<string> otherwise)
    {
        if (this == Open) onOpen();
        else if (this == Applied) onApplied();
        else if (this == Banished) onBanished();
        else if (this == Paid) onPaid();
        else otherwise(Value);
    }
}
