using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The role of the debit note.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<DebitNoteRole>))]
public sealed record DebitNoteRole : OpenStringEnum<DebitNoteRole>
{
    private DebitNoteRole(string value) : base(value)
    {
    }

    public static readonly DebitNoteRole Chargeback = new("chargeback");

    public static readonly DebitNoteRole Refund = new("refund");

    public TResult Match<TResult>(Func<TResult> onChargeback,
        Func<TResult> onRefund,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Chargeback => onChargeback(),
            _ when this == Refund => onRefund(),
            _ => otherwise(Value)
        };

    public void Match(Action onChargeback, Action onRefund, Action<string> otherwise)
    {
        if (this == Chargeback) onChargeback();
        else if (this == Refund) onRefund();
        else otherwise(Value);
    }
}
