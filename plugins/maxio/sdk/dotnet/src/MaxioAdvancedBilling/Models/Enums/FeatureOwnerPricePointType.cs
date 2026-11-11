using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// Identifies which kind of price point a feature catalog item override applies to: <c>ProductPricePoint</c> for a product price point, <c>PricePoint</c> for a component price point. Only relevant when <c>price_point_id</c> is set.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<FeatureOwnerPricePointType>))]
public sealed record FeatureOwnerPricePointType : OpenStringEnum<FeatureOwnerPricePointType>
{
    private FeatureOwnerPricePointType(string value) : base(value)
    {
    }

    public static readonly FeatureOwnerPricePointType ProductPricePoint = new("ProductPricePoint");

    public static readonly FeatureOwnerPricePointType PricePoint = new("PricePoint");

    public TResult Match<TResult>(Func<TResult> onProductPricePoint,
        Func<TResult> onPricePoint,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == ProductPricePoint => onProductPricePoint(),
            _ when this == PricePoint => onPricePoint(),
            _ => otherwise(Value)
        };

    public void Match(Action onProductPricePoint, Action onPricePoint, Action<string> otherwise)
    {
        if (this == ProductPricePoint) onProductPricePoint();
        else if (this == PricePoint) onPricePoint();
        else otherwise(Value);
    }
}
