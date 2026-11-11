using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<ListEventsDateField>))]
public sealed record ListEventsDateField : OpenStringEnum<ListEventsDateField>
{
    private ListEventsDateField(string value) : base(value)
    {
    }

    public static readonly ListEventsDateField CreatedAt = new("created_at");

    public TResult Match<TResult>(Func<TResult> onCreatedAt, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == CreatedAt => onCreatedAt(),
            _ => otherwise(Value)
        };

    public void Match(Action onCreatedAt, Action<string> otherwise)
    {
        if (this == CreatedAt) onCreatedAt();
        else otherwise(Value);
    }
}
