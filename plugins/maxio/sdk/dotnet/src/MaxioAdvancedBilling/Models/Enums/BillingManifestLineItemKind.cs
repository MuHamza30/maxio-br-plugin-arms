using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// A handle for the billing manifest line item kind
/// </summary>
[JsonConverter(typeof(StringEnumConverter<BillingManifestLineItemKind>))]
public sealed record BillingManifestLineItemKind : OpenStringEnum<BillingManifestLineItemKind>
{
    private BillingManifestLineItemKind(string value) : base(value)
    {
    }

    public static readonly BillingManifestLineItemKind Baseline = new("baseline");

    public static readonly BillingManifestLineItemKind Initial = new("initial");

    public static readonly BillingManifestLineItemKind Trial = new("trial");

    public static readonly BillingManifestLineItemKind Coupon = new("coupon");

    public static readonly BillingManifestLineItemKind Component = new("component");

    public static readonly BillingManifestLineItemKind Tax = new("tax");

    public TResult Match<TResult>(Func<TResult> onBaseline,
        Func<TResult> onInitial,
        Func<TResult> onTrial,
        Func<TResult> onCoupon,
        Func<TResult> onComponent,
        Func<TResult> onTax,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Baseline => onBaseline(),
            _ when this == Initial => onInitial(),
            _ when this == Trial => onTrial(),
            _ when this == Coupon => onCoupon(),
            _ when this == Component => onComponent(),
            _ when this == Tax => onTax(),
            _ => otherwise(Value)
        };

    public void Match(Action onBaseline,
        Action onInitial,
        Action onTrial,
        Action onCoupon,
        Action onComponent,
        Action onTax,
        Action<string> otherwise)
    {
        if (this == Baseline) onBaseline();
        else if (this == Initial) onInitial();
        else if (this == Trial) onTrial();
        else if (this == Coupon) onCoupon();
        else if (this == Component) onComponent();
        else if (this == Tax) onTax();
        else otherwise(Value);
    }
}
