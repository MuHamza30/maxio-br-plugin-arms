using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// The identifier for the pricing scheme. See <see href="https://help.chargify.com/products/product-components.html">Product Components</see> for an overview of pricing schemes.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<PricingScheme>))]
public sealed record PricingScheme : OpenStringEnum<PricingScheme>
{
    private PricingScheme(string value) : base(value)
    {
    }

    public static readonly PricingScheme Stairstep = new("stairstep");

    public static readonly PricingScheme Volume = new("volume");

    public static readonly PricingScheme PerUnit = new("per_unit");

    public static readonly PricingScheme Tiered = new("tiered");

    public TResult Match<TResult>(Func<TResult> onStairstep,
        Func<TResult> onVolume,
        Func<TResult> onPerUnit,
        Func<TResult> onTiered,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Stairstep => onStairstep(),
            _ when this == Volume => onVolume(),
            _ when this == PerUnit => onPerUnit(),
            _ when this == Tiered => onTiered(),
            _ => otherwise(Value)
        };

    public void Match(Action onStairstep, Action onVolume, Action onPerUnit, Action onTiered, Action<string> otherwise)
    {
        if (this == Stairstep) onStairstep();
        else if (this == Volume) onVolume();
        else if (this == PerUnit) onPerUnit();
        else if (this == Tiered) onTiered();
        else otherwise(Value);
    }
}
