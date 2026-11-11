using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// Passed as a parameter to list methods to return only non null values.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<IncludeNotNull>))]
public sealed record IncludeNotNull : OpenStringEnum<IncludeNotNull>
{
    private IncludeNotNull(string value) : base(value)
    {
    }

    public static readonly IncludeNotNull NotNull = new("not_null");

    public TResult Match<TResult>(Func<TResult> onNotNull, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == NotNull => onNotNull(),
            _ => otherwise(Value)
        };

    public void Match(Action onNotNull, Action<string> otherwise)
    {
        if (this == NotNull) onNotNull();
        else otherwise(Value);
    }
}
