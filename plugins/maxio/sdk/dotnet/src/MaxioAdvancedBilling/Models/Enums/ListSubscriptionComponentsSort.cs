using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<ListSubscriptionComponentsSort>))]
public sealed record ListSubscriptionComponentsSort : OpenStringEnum<ListSubscriptionComponentsSort>
{
    private ListSubscriptionComponentsSort(string value) : base(value)
    {
    }

    public static readonly ListSubscriptionComponentsSort Id = new("id");

    public static readonly ListSubscriptionComponentsSort UpdatedAt = new("updated_at");

    public TResult Match<TResult>(Func<TResult> onId, Func<TResult> onUpdatedAt, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Id => onId(),
            _ when this == UpdatedAt => onUpdatedAt(),
            _ => otherwise(Value)
        };

    public void Match(Action onId, Action onUpdatedAt, Action<string> otherwise)
    {
        if (this == Id) onId();
        else if (this == UpdatedAt) onUpdatedAt();
        else otherwise(Value);
    }
}
