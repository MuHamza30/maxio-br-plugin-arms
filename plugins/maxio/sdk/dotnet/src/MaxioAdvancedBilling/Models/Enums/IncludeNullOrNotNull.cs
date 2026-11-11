using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// Allows to filter by <c>not_null</c> or <c>null</c>.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<IncludeNullOrNotNull>))]
public sealed record IncludeNullOrNotNull : OpenStringEnum<IncludeNullOrNotNull>
{
    private IncludeNullOrNotNull(string value) : base(value)
    {
    }

    public static readonly IncludeNullOrNotNull NotNull = new("not_null");

    public static readonly IncludeNullOrNotNull Null = new("null");

    public TResult Match<TResult>(Func<TResult> onNotNull, Func<TResult> onNull, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == NotNull => onNotNull(),
            _ when this == Null => onNull(),
            _ => otherwise(Value)
        };

    public void Match(Action onNotNull, Action onNull, Action<string> otherwise)
    {
        if (this == NotNull) onNotNull();
        else if (this == Null) onNull();
        else otherwise(Value);
    }
}
