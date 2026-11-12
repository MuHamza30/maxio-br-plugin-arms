using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class DeletePrepaidUsageAllocationError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private readonly Optional<SubscriptionComponentAllocationError1> _subscriptionComponentAllocationError1Value;

    private DeletePrepaidUsageAllocationError(Optional<RawError> noContentValue,
        Optional<SubscriptionComponentAllocationError1> subscriptionComponentAllocationError1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
        _subscriptionComponentAllocationError1Value = subscriptionComponentAllocationError1Value;
    }

    private static DeletePrepaidUsageAllocationError AsNoContent(RawError value) =>
        new(Optional<RawError>.Some(value), default, default);

    private static DeletePrepaidUsageAllocationError AsSubscriptionComponentAllocationError1(SubscriptionComponentAllocationError1 value) =>
        new(default, Optional<SubscriptionComponentAllocationError1>.Some(value), default);

    private static DeletePrepaidUsageAllocationError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetNoContent([NotNullWhen(true)] out RawError? value) => _noContentValue.TryGetValue(out value);

    public bool TryGetSubscriptionComponentAllocationError1([NotNullWhen(true)] out SubscriptionComponentAllocationError1? value) =>
        _subscriptionComponentAllocationError1Value.TryGetValue(out value);

    public TResult Match<TResult>(Func<RawError, TResult> onNoContent,
        Func<SubscriptionComponentAllocationError1, TResult> onSubscriptionComponentAllocationError1,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetNoContent(out var noContentValue) => onNoContent(noContentValue),
            _ when TryGetSubscriptionComponentAllocationError1(
                out var subscriptionComponentAllocationError1Value) => onSubscriptionComponentAllocationError1(
                subscriptionComponentAllocationError1Value),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(DeletePrepaidUsageAllocationError)} holds no value.")
        };

    public void Match(Action<RawError> onNoContent,
        Action<SubscriptionComponentAllocationError1> onSubscriptionComponentAllocationError1,
        Action<RawError> otherwise)
    {
        if (TryGetNoContent(out var noContentValue)) onNoContent(noContentValue);
        else if (TryGetSubscriptionComponentAllocationError1(
            out var subscriptionComponentAllocationError1Value)) onSubscriptionComponentAllocationError1(
            subscriptionComponentAllocationError1Value);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(DeletePrepaidUsageAllocationError)} holds no value.");
    }

    private static Task<DeletePrepaidUsageAllocationError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            422 => response.Json<SubscriptionComponentAllocationError1>().As(AsSubscriptionComponentAllocationError1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<DeletePrepaidUsageAllocationError> Response { get; } = new(Create);
}
