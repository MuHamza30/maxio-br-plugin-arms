using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<ListPrepaymentDateField>))]
public sealed record ListPrepaymentDateField : OpenStringEnum<ListPrepaymentDateField>
{
    private ListPrepaymentDateField(string value) : base(value)
    {
    }

    public static readonly ListPrepaymentDateField CreatedAt = new("created_at");

    public static readonly ListPrepaymentDateField ApplicationAt = new("application_at");

    public TResult Match<TResult>(Func<TResult> onCreatedAt,
        Func<TResult> onApplicationAt,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == CreatedAt => onCreatedAt(),
            _ when this == ApplicationAt => onApplicationAt(),
            _ => otherwise(Value)
        };

    public void Match(Action onCreatedAt, Action onApplicationAt, Action<string> otherwise)
    {
        if (this == CreatedAt) onCreatedAt();
        else if (this == ApplicationAt) onApplicationAt();
        else otherwise(Value);
    }
}
