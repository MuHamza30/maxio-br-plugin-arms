using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<Status>))]
public sealed record Status : OpenStringEnum<Status>
{
    private Status(string value) : base(value)
    {
    }

    public static readonly Status Draft = new("draft");

    public static readonly Status Scheduled = new("scheduled");

    public static readonly Status Pending = new("pending");

    public static readonly Status Canceled = new("canceled");

    public static readonly Status Active = new("active");

    public static readonly Status Fulfilled = new("fulfilled");

    public TResult Match<TResult>(Func<TResult> onDraft,
        Func<TResult> onScheduled,
        Func<TResult> onPending,
        Func<TResult> onCanceled,
        Func<TResult> onActive,
        Func<TResult> onFulfilled,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Draft => onDraft(),
            _ when this == Scheduled => onScheduled(),
            _ when this == Pending => onPending(),
            _ when this == Canceled => onCanceled(),
            _ when this == Active => onActive(),
            _ when this == Fulfilled => onFulfilled(),
            _ => otherwise(Value)
        };

    public void Match(Action onDraft,
        Action onScheduled,
        Action onPending,
        Action onCanceled,
        Action onActive,
        Action onFulfilled,
        Action<string> otherwise)
    {
        if (this == Draft) onDraft();
        else if (this == Scheduled) onScheduled();
        else if (this == Pending) onPending();
        else if (this == Canceled) onCanceled();
        else if (this == Active) onActive();
        else if (this == Fulfilled) onFulfilled();
        else otherwise(Value);
    }
}
