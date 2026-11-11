using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// Used for sorting results.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<SortingDirection>))]
public sealed record SortingDirection : OpenStringEnum<SortingDirection>
{
    private SortingDirection(string value) : base(value)
    {
    }

    public static readonly SortingDirection Asc = new("asc");

    public static readonly SortingDirection Desc = new("desc");

    public TResult Match<TResult>(Func<TResult> onAsc, Func<TResult> onDesc, Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Asc => onAsc(),
            _ when this == Desc => onDesc(),
            _ => otherwise(Value)
        };

    public void Match(Action onAsc, Action onDesc, Action<string> otherwise)
    {
        if (this == Asc) onAsc();
        else if (this == Desc) onDesc();
        else otherwise(Value);
    }
}
