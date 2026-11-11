using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<SortDirection>))]
public sealed record SortDirection : OpenStringEnum<SortDirection>
{
    private SortDirection(string value) : base(value)
    {
    }

    public static readonly SortDirection Asc = new("asc");

    public static readonly SortDirection Desc = new("desc");

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
