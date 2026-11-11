using System;
using System.Text.Json.Serialization;
using MaxioAdvancedBilling.Core.Enum;

namespace MaxioAdvancedBilling.Models.Enums;

/// <summary>
/// <c>active</c>: refilling normally. <c>erroring</c>: the schedule is still due and retrying, but the last attempt couldn't be written (see <c>reason</c>); correcting the underlying entitlement resumes funding on the next run with no action needed on the wallet itself. <c>stopped</c>: funding has ended for good (see <c>reason</c>); units already granted stay on the wallet and remain spendable, but no further automatic grants will be made.
/// </summary>
[JsonConverter(typeof(StringEnumConverter<WalletRecurringFundingStatus>))]
public sealed record WalletRecurringFundingStatus : OpenStringEnum<WalletRecurringFundingStatus>
{
    private WalletRecurringFundingStatus(string value) : base(value)
    {
    }

    public static readonly WalletRecurringFundingStatus Active = new("active");

    public static readonly WalletRecurringFundingStatus Erroring = new("erroring");

    public static readonly WalletRecurringFundingStatus Stopped = new("stopped");

    public TResult Match<TResult>(Func<TResult> onActive,
        Func<TResult> onErroring,
        Func<TResult> onStopped,
        Func<string, TResult> otherwise) =>
        this switch
        {
            _ when this == Active => onActive(),
            _ when this == Erroring => onErroring(),
            _ when this == Stopped => onStopped(),
            _ => otherwise(Value)
        };

    public void Match(Action onActive, Action onErroring, Action onStopped, Action<string> otherwise)
    {
        if (this == Active) onActive();
        else if (this == Erroring) onErroring();
        else if (this == Stopped) onStopped();
        else otherwise(Value);
    }
}
