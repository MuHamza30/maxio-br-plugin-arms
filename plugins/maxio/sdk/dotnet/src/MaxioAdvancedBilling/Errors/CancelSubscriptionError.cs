using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Errors;

public sealed class CancelSubscriptionError : ApiError
{
    private readonly Optional<CancelSubscriptionErrorResponse> _cancelSubscriptionErrorResponseValue;

    private readonly Optional<RawError> _noContentValue;

    private CancelSubscriptionError(Optional<CancelSubscriptionErrorResponse> cancelSubscriptionErrorResponseValue,
        Optional<RawError> noContentValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _cancelSubscriptionErrorResponseValue = cancelSubscriptionErrorResponseValue;
        _noContentValue = noContentValue;
    }

    private static CancelSubscriptionError AsCancelSubscriptionErrorResponse(CancelSubscriptionErrorResponse value) =>
        new(Optional<CancelSubscriptionErrorResponse>.Some(value), default, default);

    private static CancelSubscriptionError AsNoContent(RawError value) =>
        new(default, Optional<RawError>.Some(value), default);

    private static CancelSubscriptionError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetCancelSubscriptionErrorResponse([NotNullWhen(true)] out CancelSubscriptionErrorResponse? value) =>
        _cancelSubscriptionErrorResponseValue.TryGetValue(out value);

    public bool TryGetNoContent([NotNullWhen(true)] out RawError? value) => _noContentValue.TryGetValue(out value);

    public TResult Match<TResult>(Func<CancelSubscriptionErrorResponse, TResult> onCancelSubscriptionErrorResponse,
        Func<RawError, TResult> onNoContent,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetCancelSubscriptionErrorResponse(
                out var cancelSubscriptionErrorResponseValue) => onCancelSubscriptionErrorResponse(
                cancelSubscriptionErrorResponseValue),
            _ when TryGetNoContent(out var noContentValue) => onNoContent(noContentValue),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(CancelSubscriptionError)} holds no value.")
        };

    public void Match(Action<CancelSubscriptionErrorResponse> onCancelSubscriptionErrorResponse,
        Action<RawError> onNoContent,
        Action<RawError> otherwise)
    {
        if (TryGetCancelSubscriptionErrorResponse(
            out var cancelSubscriptionErrorResponseValue)) onCancelSubscriptionErrorResponse(
            cancelSubscriptionErrorResponseValue);
        else if (TryGetNoContent(out var noContentValue)) onNoContent(noContentValue);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(CancelSubscriptionError)} holds no value.");
    }

    private static Task<CancelSubscriptionError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            422 => response.Json<CancelSubscriptionErrorResponse>().As(AsCancelSubscriptionErrorResponse),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<CancelSubscriptionError> Response { get; } = new(Create);
}
