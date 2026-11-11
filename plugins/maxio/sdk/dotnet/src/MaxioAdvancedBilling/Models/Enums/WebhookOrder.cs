using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<WebhookOrder>))]
public sealed record WebhookOrder : OpenStringEnum<WebhookOrder>
{
    private WebhookOrder(string value) : base(value)
    {
    }

    public static readonly WebhookOrder NewestFirst = new("newest_first");

    public static readonly WebhookOrder OldestFirst = new("oldest_first");

    public TResult Match<TResult>(Func<TResult> onNewestFirst,
        Func<TResult> onOldestFirst,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == NewestFirst => onNewestFirst(),
            _ when this == OldestFirst => onOldestFirst(),
            _ => otherwise(Value)
        };

    public void Match(Action onNewestFirst, Action onOldestFirst, Action<string> otherwise)
    {
        if (this == NewestFirst) onNewestFirst();
        else if (this == OldestFirst) onOldestFirst();
        else otherwise(Value);
    }
}
