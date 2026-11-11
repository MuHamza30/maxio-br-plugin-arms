using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<SortBy>))]
public sealed record SortBy : OpenStringEnum<SortBy>
{
    private SortBy(string value) : base(value)
    {
    }

    public static readonly SortBy Name = new("name");

    public static readonly SortBy UpdatedAt = new("updated_at");

    public static readonly SortBy Kind = new("kind");

    public static readonly SortBy ValueType = new("value_type");

    public TResult Match<TResult>(Func<TResult> onName,
        Func<TResult> onUpdatedAt,
        Func<TResult> onKind,
        Func<TResult> onValueType,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Name => onName(),
            _ when this == UpdatedAt => onUpdatedAt(),
            _ when this == Kind => onKind(),
            _ when this == ValueType => onValueType(),
            _ => otherwise(Value)
        };

    public void Match(Action onName, Action onUpdatedAt, Action onKind, Action onValueType, Action<string> otherwise)
    {
        if (this == Name) onName();
        else if (this == UpdatedAt) onUpdatedAt();
        else if (this == Kind) onKind();
        else if (this == ValueType) onValueType();
        else otherwise(Value);
    }
}
