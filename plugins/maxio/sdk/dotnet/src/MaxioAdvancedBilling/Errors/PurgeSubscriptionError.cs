using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class PurgeSubscriptionError : ApiError
{
    private readonly Optional<SubscriptionResponse> _subscriptionResponseValue;

    private PurgeSubscriptionError(Optional<SubscriptionResponse> subscriptionResponseValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _subscriptionResponseValue = subscriptionResponseValue;
    }

    private static PurgeSubscriptionError AsSubscriptionResponse(SubscriptionResponse value) =>
        new(Optional<SubscriptionResponse>.Some(value), default);

    private static PurgeSubscriptionError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetSubscriptionResponse([NotNullWhen(true)] out SubscriptionResponse? value) =>
        _subscriptionResponseValue.TryGetValue(out value);

    public TResult Match<TResult>(Func<SubscriptionResponse, TResult> onSubscriptionResponse,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetSubscriptionResponse(out var subscriptionResponseValue) => onSubscriptionResponse(
                subscriptionResponseValue),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(PurgeSubscriptionError)} holds no value.")
        };

    public void Match(Action<SubscriptionResponse> onSubscriptionResponse, Action<RawError> otherwise)
    {
        if (TryGetSubscriptionResponse(out var subscriptionResponseValue)) onSubscriptionResponse(
            subscriptionResponseValue);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(PurgeSubscriptionError)} holds no value.");
    }

    private static Task<PurgeSubscriptionError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            400 => response.Json<SubscriptionResponse>().As(AsSubscriptionResponse),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<PurgeSubscriptionError> Response { get; } = new(Create);
}
