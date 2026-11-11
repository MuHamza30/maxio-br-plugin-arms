using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<SortBy1>))]
public sealed record SortBy1 : OpenStringEnum<SortBy1>
{
    private SortBy1(string value) : base(value)
    {
    }

    public static readonly SortBy1 Name = new("name");

    public static readonly SortBy1 UnitName = new("unit_name");

    public static readonly SortBy1 UpdatedAt = new("updated_at");

    public TResult Match<TResult>(Func<TResult> onName,
        Func<TResult> onUnitName,
        Func<TResult> onUpdatedAt,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Name => onName(),
            _ when this == UnitName => onUnitName(),
            _ when this == UpdatedAt => onUpdatedAt(),
            _ => otherwise(Value)
        };

    public void Match(Action onName, Action onUnitName, Action onUpdatedAt, Action<string> otherwise)
    {
        if (this == Name) onName();
        else if (this == UnitName) onUnitName();
        else if (this == UpdatedAt) onUpdatedAt();
        else otherwise(Value);
    }
}
