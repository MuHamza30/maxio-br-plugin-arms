using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// Role for the price.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<CurrencyPriceRole>))]
public sealed record CurrencyPriceRole : OpenStringEnum<CurrencyPriceRole>
{
    private CurrencyPriceRole(string value) : base(value)
    {
    }

    public static readonly CurrencyPriceRole Baseline = new("baseline");

    public static readonly CurrencyPriceRole Trial = new("trial");

    public static readonly CurrencyPriceRole Initial = new("initial");

    public TResult Match<TResult>(Func<TResult> onBaseline,
        Func<TResult> onTrial,
        Func<TResult> onInitial,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Baseline => onBaseline(),
            _ when this == Trial => onTrial(),
            _ when this == Initial => onInitial(),
            _ => otherwise(Value)
        };

    public void Match(Action onBaseline, Action onTrial, Action onInitial, Action<string> otherwise)
    {
        if (this == Baseline) onBaseline();
        else if (this == Trial) onTrial();
        else if (this == Initial) onInitial();
        else otherwise(Value);
    }
}
