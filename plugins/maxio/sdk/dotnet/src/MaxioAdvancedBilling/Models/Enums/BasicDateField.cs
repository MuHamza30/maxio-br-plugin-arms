using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// Allows to filter by <c>created_at</c> or <c>updated_at</c>.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<BasicDateField>))]
public sealed record BasicDateField : OpenStringEnum<BasicDateField>
{
    private BasicDateField(string value) : base(value)
    {
    }

    public static readonly BasicDateField UpdatedAt = new("updated_at");

    public static readonly BasicDateField CreatedAt = new("created_at");

    public TResult Match<TResult>(Func<TResult> onUpdatedAt,
        Func<TResult> onCreatedAt,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == UpdatedAt => onUpdatedAt(),
            _ when this == CreatedAt => onCreatedAt(),
            _ => otherwise(Value)
        };

    public void Match(Action onUpdatedAt, Action onCreatedAt, Action<string> otherwise)
    {
        if (this == UpdatedAt) onUpdatedAt();
        else if (this == CreatedAt) onCreatedAt();
        else otherwise(Value);
    }
}
