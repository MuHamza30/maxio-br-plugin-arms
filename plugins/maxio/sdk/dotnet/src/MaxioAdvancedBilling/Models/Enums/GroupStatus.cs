using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<GroupStatus>))]
public sealed record GroupStatus : OpenStringEnum<GroupStatus>
{
    private GroupStatus(string value) : base(value)
    {
    }

    public static readonly GroupStatus Ungrouped = new("ungrouped");

    public static readonly GroupStatus Grouped = new("grouped");

    public TResult Match<TResult>(Func<TResult> onUngrouped,
        Func<TResult> onGrouped,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Ungrouped => onUngrouped(),
            _ when this == Grouped => onGrouped(),
            _ => otherwise(Value)
        };

    public void Match(Action onUngrouped, Action onGrouped, Action<string> otherwise)
    {
        if (this == Ungrouped) onUngrouped();
        else if (this == Grouped) onGrouped();
        else otherwise(Value);
    }
}
