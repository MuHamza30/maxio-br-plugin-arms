using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Errors;

public sealed class DeductServiceCreditError : ApiError
{
    private readonly Optional<DeductServiceCreditErrorResponse> _deductServiceCreditErrorResponseValue;

    private DeductServiceCreditError(Optional<DeductServiceCreditErrorResponse> deductServiceCreditErrorResponseValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _deductServiceCreditErrorResponseValue = deductServiceCreditErrorResponseValue;
    }

    private static DeductServiceCreditError AsDeductServiceCreditErrorResponse(DeductServiceCreditErrorResponse value) =>
        new(Optional<DeductServiceCreditErrorResponse>.Some(value), default);

    private static DeductServiceCreditError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetDeductServiceCreditErrorResponse([NotNullWhen(true)] out DeductServiceCreditErrorResponse? value) =>
        _deductServiceCreditErrorResponseValue.TryGetValue(out value);

    public TResult Match<TResult>(Func<DeductServiceCreditErrorResponse, TResult> onDeductServiceCreditErrorResponse,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetDeductServiceCreditErrorResponse(
                out var deductServiceCreditErrorResponseValue) => onDeductServiceCreditErrorResponse(
                deductServiceCreditErrorResponseValue),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(DeductServiceCreditError)} holds no value.")
        };

    public void Match(Action<DeductServiceCreditErrorResponse> onDeductServiceCreditErrorResponse,
        Action<RawError> otherwise)
    {
        if (TryGetDeductServiceCreditErrorResponse(
            out var deductServiceCreditErrorResponseValue)) onDeductServiceCreditErrorResponse(
            deductServiceCreditErrorResponseValue);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(DeductServiceCreditError)} holds no value.");
    }

    private static Task<DeductServiceCreditError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<DeductServiceCreditErrorResponse>().As(AsDeductServiceCreditErrorResponse),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<DeductServiceCreditError> Response { get; } = new(Create);
}
