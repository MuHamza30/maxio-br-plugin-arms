using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// A handle for the line item kind for allocation preview
/// </summary>
[JsonConverter(typeof(StringEnumConverter<AllocationPreviewLineItemKind>))]
public sealed record AllocationPreviewLineItemKind : OpenStringEnum<AllocationPreviewLineItemKind>
{
    private AllocationPreviewLineItemKind(string value) : base(value)
    {
    }

    public static readonly AllocationPreviewLineItemKind QuantityBasedComponent = new("quantity_based_component");

    public static readonly AllocationPreviewLineItemKind OnOffComponent = new("on_off_component");

    public static readonly AllocationPreviewLineItemKind Coupon = new("coupon");

    public static readonly AllocationPreviewLineItemKind Tax = new("tax");

    public TResult Match<TResult>(Func<TResult> onQuantityBasedComponent,
        Func<TResult> onOnOffComponent,
        Func<TResult> onCoupon,
        Func<TResult> onTax,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == QuantityBasedComponent => onQuantityBasedComponent(),
            _ when this == OnOffComponent => onOnOffComponent(),
            _ when this == Coupon => onCoupon(),
            _ when this == Tax => onTax(),
            _ => otherwise(Value)
        };

    public void Match(Action onQuantityBasedComponent,
        Action onOnOffComponent,
        Action onCoupon,
        Action onTax,
        Action<string> otherwise)
    {
        if (this == QuantityBasedComponent) onQuantityBasedComponent();
        else if (this == OnOffComponent) onOnOffComponent();
        else if (this == Coupon) onCoupon();
        else if (this == Tax) onTax();
        else otherwise(Value);
    }
}
