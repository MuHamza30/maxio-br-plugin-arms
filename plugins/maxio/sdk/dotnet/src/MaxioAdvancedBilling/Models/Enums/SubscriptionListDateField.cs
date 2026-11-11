using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<SubscriptionListDateField>))]
public sealed record SubscriptionListDateField : OpenStringEnum<SubscriptionListDateField>
{
    private SubscriptionListDateField(string value) : base(value)
    {
    }

    public static readonly SubscriptionListDateField UpdatedAt = new("updated_at");

    public TResult Match<TResult>(Func<TResult> onUpdatedAt, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == UpdatedAt => onUpdatedAt(),
            _ => otherwise(Value)
        };

    public void Match(Action onUpdatedAt, Action<string> otherwise)
    {
        if (this == UpdatedAt) onUpdatedAt();
        else otherwise(Value);
    }
}
