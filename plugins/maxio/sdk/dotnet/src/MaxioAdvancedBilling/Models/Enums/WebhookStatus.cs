using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<WebhookStatus>))]
public sealed record WebhookStatus : OpenStringEnum<WebhookStatus>
{
    private WebhookStatus(string value) : base(value)
    {
    }

    public static readonly WebhookStatus Successful = new("successful");

    public static readonly WebhookStatus Failed = new("failed");

    public static readonly WebhookStatus Pending = new("pending");

    public static readonly WebhookStatus Paused = new("paused");

    public TResult Match<TResult>(Func<TResult> onSuccessful,
        Func<TResult> onFailed,
        Func<TResult> onPending,
        Func<TResult> onPaused,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Successful => onSuccessful(),
            _ when this == Failed => onFailed(),
            _ when this == Pending => onPending(),
            _ when this == Paused => onPaused(),
            _ => otherwise(Value)
        };

    public void Match(Action onSuccessful, Action onFailed, Action onPending, Action onPaused, Action<string> otherwise)
    {
        if (this == Successful) onSuccessful();
        else if (this == Failed) onFailed();
        else if (this == Pending) onPending();
        else if (this == Paused) onPaused();
        else otherwise(Value);
    }
}
