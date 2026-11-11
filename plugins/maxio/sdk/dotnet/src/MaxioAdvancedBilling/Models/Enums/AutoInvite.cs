using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

[JsonConverter(typeof(IntEnumConverter<AutoInvite>))]
public sealed record AutoInvite : OpenIntEnum<AutoInvite>
{
    private AutoInvite(long value) : base(value)
    {
    }

    /// <summary>
    /// Do not send the invitation email.
    /// </summary>
    public static readonly AutoInvite Value0 = new(0L);

    /// <summary>
    /// Automatically send the invitation email.
    /// </summary>
    public static readonly AutoInvite Value1 = new(1L);

    public TResult Match<TResult>(Func<TResult> onValue0, Func<TResult> onValue1, Func<long, TResult> otherwise) =>
        this switch
        {
            _ when this == Value0 => onValue0(),
            _ when this == Value1 => onValue1(),
            _ => otherwise(Value)
        };

    public void Match(Action onValue0, Action onValue1, Action<long> otherwise)
    {
        if (this == Value0) onValue0();
        else if (this == Value1) onValue1();
        else otherwise(Value);
    }
}
