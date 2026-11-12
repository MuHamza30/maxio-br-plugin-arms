using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class RemoveCouponFromSubscriptionError : ApiError
{
    private readonly Optional<SubscriptionRemoveCouponErrors1> _subscriptionRemoveCouponErrors1Value;

    private RemoveCouponFromSubscriptionError(Optional<SubscriptionRemoveCouponErrors1> subscriptionRemoveCouponErrors1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _subscriptionRemoveCouponErrors1Value = subscriptionRemoveCouponErrors1Value;
    }

    private static RemoveCouponFromSubscriptionError AsSubscriptionRemoveCouponErrors1(SubscriptionRemoveCouponErrors1 value) =>
        new(Optional<SubscriptionRemoveCouponErrors1>.Some(value), default);

    private static RemoveCouponFromSubscriptionError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetSubscriptionRemoveCouponErrors1([NotNullWhen(true)] out SubscriptionRemoveCouponErrors1? value) =>
        _subscriptionRemoveCouponErrors1Value.TryGetValue(out value);

    public TResult Match<TResult>(Func<SubscriptionRemoveCouponErrors1, TResult> onSubscriptionRemoveCouponErrors1,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetSubscriptionRemoveCouponErrors1(
                out var subscriptionRemoveCouponErrors1Value) => onSubscriptionRemoveCouponErrors1(
                subscriptionRemoveCouponErrors1Value),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(RemoveCouponFromSubscriptionError)} holds no value.")
        };

    public void Match(Action<SubscriptionRemoveCouponErrors1> onSubscriptionRemoveCouponErrors1,
        Action<RawError> otherwise)
    {
        if (TryGetSubscriptionRemoveCouponErrors1(
            out var subscriptionRemoveCouponErrors1Value)) onSubscriptionRemoveCouponErrors1(
            subscriptionRemoveCouponErrors1Value);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(RemoveCouponFromSubscriptionError)} holds no value.");
    }

    private static Task<RemoveCouponFromSubscriptionError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<SubscriptionRemoveCouponErrors1>().As(AsSubscriptionRemoveCouponErrors1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<RemoveCouponFromSubscriptionError> Response { get; } = new(Create);
}
