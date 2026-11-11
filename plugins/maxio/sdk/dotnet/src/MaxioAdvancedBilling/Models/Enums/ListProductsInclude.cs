using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<ListProductsInclude>))]
public sealed record ListProductsInclude : OpenStringEnum<ListProductsInclude>
{
    private ListProductsInclude(string value) : base(value)
    {
    }

    public static readonly ListProductsInclude PrepaidProductPricePoint = new("prepaid_product_price_point");

    public TResult Match<TResult>(Func<TResult> onPrepaidProductPricePoint, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == PrepaidProductPricePoint => onPrepaidProductPricePoint(),
            _ => otherwise(Value)
        };

    public void Match(Action onPrepaidProductPricePoint, Action<string> otherwise)
    {
        if (this == PrepaidProductPricePoint) onPrepaidProductPricePoint();
        else otherwise(Value);
    }
}
