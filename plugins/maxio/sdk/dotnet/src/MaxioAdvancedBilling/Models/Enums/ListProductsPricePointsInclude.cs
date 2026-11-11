using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<ListProductsPricePointsInclude>))]
public sealed record ListProductsPricePointsInclude : OpenStringEnum<ListProductsPricePointsInclude>
{
    private ListProductsPricePointsInclude(string value) : base(value)
    {
    }

    public static readonly ListProductsPricePointsInclude CurrencyPrices = new("currency_prices");

    public TResult Match<TResult>(Func<TResult> onCurrencyPrices, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == CurrencyPrices => onCurrencyPrices(),
            _ => otherwise(Value)
        };

    public void Match(Action onCurrencyPrices, Action<string> otherwise)
    {
        if (this == CurrencyPrices) onCurrencyPrices();
        else otherwise(Value);
    }
}
