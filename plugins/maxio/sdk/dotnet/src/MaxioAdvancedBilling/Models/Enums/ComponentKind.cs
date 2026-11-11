using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// A handle for the component type
/// </summary>
[JsonConverter(typeof(StringEnumConverter<ComponentKind>))]
public sealed record ComponentKind : OpenStringEnum<ComponentKind>
{
    private ComponentKind(string value) : base(value)
    {
    }

    public static readonly ComponentKind MeteredComponent = new("metered_component");

    public static readonly ComponentKind QuantityBasedComponent = new("quantity_based_component");

    public static readonly ComponentKind OnOffComponent = new("on_off_component");

    public static readonly ComponentKind PrepaidUsageComponent = new("prepaid_usage_component");

    public static readonly ComponentKind EventBasedComponent = new("event_based_component");

    public static readonly ComponentKind WalletProduct = new("wallet_product");

    public TResult Match<TResult>(Func<TResult> onMeteredComponent,
        Func<TResult> onQuantityBasedComponent,
        Func<TResult> onOnOffComponent,
        Func<TResult> onPrepaidUsageComponent,
        Func<TResult> onEventBasedComponent,
        Func<TResult> onWalletProduct,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == MeteredComponent => onMeteredComponent(),
            _ when this == QuantityBasedComponent => onQuantityBasedComponent(),
            _ when this == OnOffComponent => onOnOffComponent(),
            _ when this == PrepaidUsageComponent => onPrepaidUsageComponent(),
            _ when this == EventBasedComponent => onEventBasedComponent(),
            _ when this == WalletProduct => onWalletProduct(),
            _ => otherwise(Value)
        };

    public void Match(Action onMeteredComponent,
        Action onQuantityBasedComponent,
        Action onOnOffComponent,
        Action onPrepaidUsageComponent,
        Action onEventBasedComponent,
        Action onWalletProduct,
        Action<string> otherwise)
    {
        if (this == MeteredComponent) onMeteredComponent();
        else if (this == QuantityBasedComponent) onQuantityBasedComponent();
        else if (this == OnOffComponent) onOnOffComponent();
        else if (this == PrepaidUsageComponent) onPrepaidUsageComponent();
        else if (this == EventBasedComponent) onEventBasedComponent();
        else if (this == WalletProduct) onWalletProduct();
        else otherwise(Value);
    }
}
