using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<AllocationPreviewDirection>))]
public sealed record AllocationPreviewDirection : OpenStringEnum<AllocationPreviewDirection>
{
    private AllocationPreviewDirection(string value) : base(value)
    {
    }

    public static readonly AllocationPreviewDirection Upgrade = new("upgrade");

    public static readonly AllocationPreviewDirection Downgrade = new("downgrade");

    public TResult Match<TResult>(Func<TResult> onUpgrade,
        Func<TResult> onDowngrade,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Upgrade => onUpgrade(),
            _ when this == Downgrade => onDowngrade(),
            _ => otherwise(Value)
        };

    public void Match(Action onUpgrade, Action onDowngrade, Action<string> otherwise)
    {
        if (this == Upgrade) onUpgrade();
        else if (this == Downgrade) onDowngrade();
        else otherwise(Value);
    }
}
