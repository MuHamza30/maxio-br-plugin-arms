using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// A handle for the line item kind
/// </summary>
[JsonConverter(typeof(StringEnumConverter<LineItemKind>))]
public sealed record LineItemKind : OpenStringEnum<LineItemKind>
{
    private LineItemKind(string value) : base(value)
    {
    }

    public static readonly LineItemKind Baseline = new("baseline");

    public static readonly LineItemKind Initial = new("initial");

    public static readonly LineItemKind Trial = new("trial");

    public static readonly LineItemKind QuantityBasedComponent = new("quantity_based_component");

    public static readonly LineItemKind PrepaidUsageComponent = new("prepaid_usage_component");

    public static readonly LineItemKind OnOffComponent = new("on_off_component");

    public static readonly LineItemKind MeteredComponent = new("metered_component");

    public static readonly LineItemKind EventBasedComponent = new("event_based_component");

    public static readonly LineItemKind Coupon = new("coupon");

    public static readonly LineItemKind Tax = new("tax");

    public TResult Match<TResult>(Func<TResult> onBaseline,
        Func<TResult> onInitial,
        Func<TResult> onTrial,
        Func<TResult> onQuantityBasedComponent,
        Func<TResult> onPrepaidUsageComponent,
        Func<TResult> onOnOffComponent,
        Func<TResult> onMeteredComponent,
        Func<TResult> onEventBasedComponent,
        Func<TResult> onCoupon,
        Func<TResult> onTax,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Baseline => onBaseline(),
            _ when this == Initial => onInitial(),
            _ when this == Trial => onTrial(),
            _ when this == QuantityBasedComponent => onQuantityBasedComponent(),
            _ when this == PrepaidUsageComponent => onPrepaidUsageComponent(),
            _ when this == OnOffComponent => onOnOffComponent(),
            _ when this == MeteredComponent => onMeteredComponent(),
            _ when this == EventBasedComponent => onEventBasedComponent(),
            _ when this == Coupon => onCoupon(),
            _ when this == Tax => onTax(),
            _ => otherwise(Value)
        };

    public void Match(Action onBaseline,
        Action onInitial,
        Action onTrial,
        Action onQuantityBasedComponent,
        Action onPrepaidUsageComponent,
        Action onOnOffComponent,
        Action onMeteredComponent,
        Action onEventBasedComponent,
        Action onCoupon,
        Action onTax,
        Action<string> otherwise)
    {
        if (this == Baseline) onBaseline();
        else if (this == Initial) onInitial();
        else if (this == Trial) onTrial();
        else if (this == QuantityBasedComponent) onQuantityBasedComponent();
        else if (this == PrepaidUsageComponent) onPrepaidUsageComponent();
        else if (this == OnOffComponent) onOnOffComponent();
        else if (this == MeteredComponent) onMeteredComponent();
        else if (this == EventBasedComponent) onEventBasedComponent();
        else if (this == Coupon) onCoupon();
        else if (this == Tax) onTax();
        else otherwise(Value);
    }
}
