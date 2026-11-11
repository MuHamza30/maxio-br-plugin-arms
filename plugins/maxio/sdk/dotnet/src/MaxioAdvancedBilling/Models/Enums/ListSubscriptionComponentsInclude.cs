using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<ListSubscriptionComponentsInclude>))]
public sealed record ListSubscriptionComponentsInclude : OpenStringEnum<ListSubscriptionComponentsInclude>
{
    private ListSubscriptionComponentsInclude(string value) : base(value)
    {
    }

    public static readonly ListSubscriptionComponentsInclude Subscription = new("subscription");

    public static readonly ListSubscriptionComponentsInclude HistoricUsages = new("historic_usages");

    public TResult Match<TResult>(Func<TResult> onSubscription,
        Func<TResult> onHistoricUsages,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Subscription => onSubscription(),
            _ when this == HistoricUsages => onHistoricUsages(),
            _ => otherwise(Value)
        };

    public void Match(Action onSubscription, Action onHistoricUsages, Action<string> otherwise)
    {
        if (this == Subscription) onSubscription();
        else if (this == HistoricUsages) onHistoricUsages();
        else otherwise(Value);
    }
}
