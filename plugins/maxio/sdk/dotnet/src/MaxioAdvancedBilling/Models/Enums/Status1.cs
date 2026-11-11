using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<Status1>))]
public sealed record Status1 : OpenStringEnum<Status1>
{
    private Status1(string value) : base(value)
    {
    }

    public static readonly Status1 Active = new("active");

    public static readonly Status1 Archived = new("archived");

    public static readonly Status1 All = new("all");

    public TResult Match<TResult>(Func<TResult> onActive,
        Func<TResult> onArchived,
        Func<TResult> onAll,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Active => onActive(),
            _ when this == Archived => onArchived(),
            _ when this == All => onAll(),
            _ => otherwise(Value)
        };

    public void Match(Action onActive, Action onArchived, Action onAll, Action<string> otherwise)
    {
        if (this == Active) onActive();
        else if (this == Archived) onArchived();
        else if (this == All) onAll();
        else otherwise(Value);
    }
}
