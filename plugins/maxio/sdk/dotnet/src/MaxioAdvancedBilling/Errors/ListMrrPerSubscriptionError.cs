using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ListMrrPerSubscriptionError : ApiError
{
    private readonly Optional<SubscriptionsMrrErrorResponse1> _subscriptionsMrrErrorResponse1Value;

    private ListMrrPerSubscriptionError(Optional<SubscriptionsMrrErrorResponse1> subscriptionsMrrErrorResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _subscriptionsMrrErrorResponse1Value = subscriptionsMrrErrorResponse1Value;
    }

    private static ListMrrPerSubscriptionError AsSubscriptionsMrrErrorResponse1(SubscriptionsMrrErrorResponse1 value) =>
        new(Optional<SubscriptionsMrrErrorResponse1>.Some(value), default);

    private static ListMrrPerSubscriptionError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetSubscriptionsMrrErrorResponse1([NotNullWhen(true)] out SubscriptionsMrrErrorResponse1? value) =>
        _subscriptionsMrrErrorResponse1Value.TryGetValue(out value);

    public TResult Match<TResult>(Func<SubscriptionsMrrErrorResponse1, TResult> onSubscriptionsMrrErrorResponse1,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetSubscriptionsMrrErrorResponse1(
                out var subscriptionsMrrErrorResponse1Value) => onSubscriptionsMrrErrorResponse1(
                subscriptionsMrrErrorResponse1Value),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(ListMrrPerSubscriptionError)} holds no value.")
        };

    public void Match(Action<SubscriptionsMrrErrorResponse1> onSubscriptionsMrrErrorResponse1,
        Action<RawError> otherwise)
    {
        if (TryGetSubscriptionsMrrErrorResponse1(
            out var subscriptionsMrrErrorResponse1Value)) onSubscriptionsMrrErrorResponse1(
            subscriptionsMrrErrorResponse1Value);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(ListMrrPerSubscriptionError)} holds no value.");
    }

    private static Task<ListMrrPerSubscriptionError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            400 => response.Json<SubscriptionsMrrErrorResponse1>().As(AsSubscriptionsMrrErrorResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ListMrrPerSubscriptionError> Response { get; } = new(Create);
}
