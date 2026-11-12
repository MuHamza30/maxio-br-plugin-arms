using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Errors;

public sealed class UpdatePrepaidSubscriptionConfigurationError : ApiError
{
    private readonly Optional<PrepaidConfigurationErrorResponse> _prepaidConfigurationErrorResponseValue;

    private UpdatePrepaidSubscriptionConfigurationError(Optional<PrepaidConfigurationErrorResponse> prepaidConfigurationErrorResponseValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _prepaidConfigurationErrorResponseValue = prepaidConfigurationErrorResponseValue;
    }

    private static UpdatePrepaidSubscriptionConfigurationError AsPrepaidConfigurationErrorResponse(PrepaidConfigurationErrorResponse value) =>
        new(Optional<PrepaidConfigurationErrorResponse>.Some(value), default);

    private static UpdatePrepaidSubscriptionConfigurationError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetPrepaidConfigurationErrorResponse([NotNullWhen(true)] out PrepaidConfigurationErrorResponse? value) =>
        _prepaidConfigurationErrorResponseValue.TryGetValue(out value);

    public TResult Match<TResult>(Func<PrepaidConfigurationErrorResponse, TResult> onPrepaidConfigurationErrorResponse,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetPrepaidConfigurationErrorResponse(
                out var prepaidConfigurationErrorResponseValue) => onPrepaidConfigurationErrorResponse(
                prepaidConfigurationErrorResponseValue),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException(
                $"{nameof(UpdatePrepaidSubscriptionConfigurationError)} holds no value.")
        };

    public void Match(Action<PrepaidConfigurationErrorResponse> onPrepaidConfigurationErrorResponse,
        Action<RawError> otherwise)
    {
        if (TryGetPrepaidConfigurationErrorResponse(
            out var prepaidConfigurationErrorResponseValue)) onPrepaidConfigurationErrorResponse(
            prepaidConfigurationErrorResponseValue);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException(
            $"{nameof(UpdatePrepaidSubscriptionConfigurationError)} holds no value.");
    }

    private static Task<UpdatePrepaidSubscriptionConfigurationError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<PrepaidConfigurationErrorResponse>().As(AsPrepaidConfigurationErrorResponse),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<UpdatePrepaidSubscriptionConfigurationError> Response { get; } = new(Create);
}
