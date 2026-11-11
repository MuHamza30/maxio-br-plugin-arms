using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// Indicates the type of metafield. A text metafield allows any string value. Dropdown and radio metafields have a set of values that can be selected. Defaults to 'text'.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<MetafieldInput>))]
public sealed record MetafieldInput : OpenStringEnum<MetafieldInput>
{
    private MetafieldInput(string value) : base(value)
    {
    }

    public static readonly MetafieldInput BalanceTracker = new("balance_tracker");

    public static readonly MetafieldInput Text = new("text");

    public static readonly MetafieldInput Radio = new("radio");

    public static readonly MetafieldInput Dropdown = new("dropdown");

    public TResult Match<TResult>(Func<TResult> onBalanceTracker,
        Func<TResult> onText,
        Func<TResult> onRadio,
        Func<TResult> onDropdown,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == BalanceTracker => onBalanceTracker(),
            _ when this == Text => onText(),
            _ when this == Radio => onRadio(),
            _ when this == Dropdown => onDropdown(),
            _ => otherwise(Value)
        };

    public void Match(Action onBalanceTracker,
        Action onText,
        Action onRadio,
        Action onDropdown,
        Action<string> otherwise)
    {
        if (this == BalanceTracker) onBalanceTracker();
        else if (this == Text) onText();
        else if (this == Radio) onRadio();
        else if (this == Dropdown) onDropdown();
        else otherwise(Value);
    }
}
