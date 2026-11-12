using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Errors;

public sealed class RefundPrepaymentError : ApiError
{
    private readonly Optional<RefundPrepaymentBaseErrorsResponse1> _refundPrepaymentBaseErrorsResponse1Value;

    private readonly Optional<RefundPrepaymentErrorResponse> _refundPrepaymentErrorResponseValue;

    private readonly Optional<string> _stringValue;

    private RefundPrepaymentError(Optional<RefundPrepaymentBaseErrorsResponse1> refundPrepaymentBaseErrorsResponse1Value,
        Optional<RefundPrepaymentErrorResponse> refundPrepaymentErrorResponseValue,
        Optional<string> stringValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _refundPrepaymentBaseErrorsResponse1Value = refundPrepaymentBaseErrorsResponse1Value;
        _refundPrepaymentErrorResponseValue = refundPrepaymentErrorResponseValue;
        _stringValue = stringValue;
    }

    private static RefundPrepaymentError AsRefundPrepaymentBaseErrorsResponse1(RefundPrepaymentBaseErrorsResponse1 value) =>
        new(Optional<RefundPrepaymentBaseErrorsResponse1>.Some(value), default, default, default);

    private static RefundPrepaymentError AsRefundPrepaymentErrorResponse(RefundPrepaymentErrorResponse value) =>
        new(default, Optional<RefundPrepaymentErrorResponse>.Some(value), default, default);

    private static RefundPrepaymentError AsString(string value) =>
        new(default, default, Optional<string>.Some(value), default);

    private static RefundPrepaymentError AsFallback(RawError value) =>
        new(default, default, default, Optional<RawError>.Some(value));

    public bool TryGetRefundPrepaymentBaseErrorsResponse1([NotNullWhen(true)] out RefundPrepaymentBaseErrorsResponse1? value) =>
        _refundPrepaymentBaseErrorsResponse1Value.TryGetValue(out value);

    public bool TryGetRefundPrepaymentErrorResponse([NotNullWhen(true)] out RefundPrepaymentErrorResponse? value) =>
        _refundPrepaymentErrorResponseValue.TryGetValue(out value);

    public bool TryGetString([NotNullWhen(true)] out string? value) => _stringValue.TryGetValue(out value);

    public TResult Match<TResult>(Func<RefundPrepaymentBaseErrorsResponse1, TResult> onRefundPrepaymentBaseErrorsResponse1,
        Func<RefundPrepaymentErrorResponse, TResult> onRefundPrepaymentErrorResponse,
        Func<string, TResult> onString,
        Func<RawError, TResult> otherwise) =>
        this switch
        {
            _ when TryGetRefundPrepaymentBaseErrorsResponse1(
                out var refundPrepaymentBaseErrorsResponse1Value) => onRefundPrepaymentBaseErrorsResponse1(
                refundPrepaymentBaseErrorsResponse1Value),
            _ when TryGetRefundPrepaymentErrorResponse(
                out var refundPrepaymentErrorResponseValue) => onRefundPrepaymentErrorResponse(
                refundPrepaymentErrorResponseValue),
            _ when TryGetString(out var stringValue) => onString(stringValue),
            _ when TryGetRawError(out var fallback) => otherwise(fallback),
            _ => throw new InvalidOperationException($"{nameof(RefundPrepaymentError)} holds no value.")
        };

    public void Match(Action<RefundPrepaymentBaseErrorsResponse1> onRefundPrepaymentBaseErrorsResponse1,
        Action<RefundPrepaymentErrorResponse> onRefundPrepaymentErrorResponse,
        Action<string> onString,
        Action<RawError> otherwise)
    {
        if (TryGetRefundPrepaymentBaseErrorsResponse1(
            out var refundPrepaymentBaseErrorsResponse1Value)) onRefundPrepaymentBaseErrorsResponse1(
            refundPrepaymentBaseErrorsResponse1Value);
        else if (TryGetRefundPrepaymentErrorResponse(
            out var refundPrepaymentErrorResponseValue)) onRefundPrepaymentErrorResponse(
            refundPrepaymentErrorResponseValue);
        else if (TryGetString(out var stringValue)) onString(stringValue);
        else if (TryGetRawError(out var fallback)) otherwise(fallback);
        else throw new InvalidOperationException($"{nameof(RefundPrepaymentError)} holds no value.");
    }

    private static Task<RefundPrepaymentError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            400 => response.Json<RefundPrepaymentBaseErrorsResponse1>().As(AsRefundPrepaymentBaseErrorsResponse1),
            404 => response.Json<string>().As(AsString),
            422 => response.Json<RefundPrepaymentErrorResponse>().As(AsRefundPrepaymentErrorResponse),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<RefundPrepaymentError> Response { get; } = new(Create);
}
