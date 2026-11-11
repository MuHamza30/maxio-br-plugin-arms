using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The current chargeback status.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<ChargebackStatus>))]
public sealed record ChargebackStatus : OpenStringEnum<ChargebackStatus>
{
    private ChargebackStatus(string value) : base(value)
    {
    }

    public static readonly ChargebackStatus Open = new("open");

    public static readonly ChargebackStatus Lost = new("lost");

    public static readonly ChargebackStatus Won = new("won");

    public static readonly ChargebackStatus Closed = new("closed");

    public TResult Match<TResult>(Func<TResult> onOpen,
        Func<TResult> onLost,
        Func<TResult> onWon,
        Func<TResult> onClosed,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Open => onOpen(),
            _ when this == Lost => onLost(),
            _ when this == Won => onWon(),
            _ when this == Closed => onClosed(),
            _ => otherwise(Value)
        };

    public void Match(Action onOpen, Action onLost, Action onWon, Action onClosed, Action<string> otherwise)
    {
        if (this == Open) onOpen();
        else if (this == Lost) onLost();
        else if (this == Won) onWon();
        else if (this == Closed) onClosed();
        else otherwise(Value);
    }
}
