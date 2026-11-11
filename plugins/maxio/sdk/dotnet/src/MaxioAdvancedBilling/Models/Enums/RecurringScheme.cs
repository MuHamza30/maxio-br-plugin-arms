using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(StringEnumConverter<RecurringScheme>))]
public sealed record RecurringScheme : OpenStringEnum<RecurringScheme>
{
    private RecurringScheme(string value) : base(value)
    {
    }

    public static readonly RecurringScheme DoNotRecur = new("do_not_recur");

    public static readonly RecurringScheme RecurIndefinitely = new("recur_indefinitely");

    public static readonly RecurringScheme RecurWithDuration = new("recur_with_duration");

    public TResult Match<TResult>(Func<TResult> onDoNotRecur,
        Func<TResult> onRecurIndefinitely,
        Func<TResult> onRecurWithDuration,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == DoNotRecur => onDoNotRecur(),
            _ when this == RecurIndefinitely => onRecurIndefinitely(),
            _ when this == RecurWithDuration => onRecurWithDuration(),
            _ => otherwise(Value)
        };

    public void Match(Action onDoNotRecur,
        Action onRecurIndefinitely,
        Action onRecurWithDuration,
        Action<string> otherwise)
    {
        if (this == DoNotRecur) onDoNotRecur();
        else if (this == RecurIndefinitely) onRecurIndefinitely();
        else if (this == RecurWithDuration) onRecurWithDuration();
        else otherwise(Value);
    }
}
