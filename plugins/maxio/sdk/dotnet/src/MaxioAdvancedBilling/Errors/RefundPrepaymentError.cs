using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;
using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Errors;

public sealed class RefundPrepaymentError : ApiError
{
    private readonly Optional<RefundPrepaymentBaseErrorsResponse1> _refundPrepaymentBaseErrorsResponse1Value;

    private readonly Optional<string> _stringValue;

    private readonly Optional<RefundPrepaymentErrorResponse> _refundPrepaymentErrorResponseValue;

    private RefundPrepaymentError(Optional<RefundPrepaymentBaseErrorsResponse1> refundPrepaymentBaseErrorsResponse1Value,
        Optional<string> stringValue,
        Optional<RefundPrepaymentErrorResponse> refundPrepaymentErrorResponseValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _refundPrepaymentBaseErrorsResponse1Value = refundPrepaymentBaseErrorsResponse1Value;
        _stringValue = stringValue;
        _refundPrepaymentErrorResponseValue = refundPrepaymentErrorResponseValue;
    }

    private static RefundPrepaymentError AsRefundPrepaymentBaseErrorsResponse1(RefundPrepaymentBaseErrorsResponse1 value) =>
        new(Optional<RefundPrepaymentBaseErrorsResponse1>.Some(value), default, default, default);

    private static RefundPrepaymentError AsString(string value) =>
        new(default, Optional<string>.Some(value), default, default);

    private static RefundPrepaymentError AsRefundPrepaymentErrorResponse(RefundPrepaymentErrorResponse value) =>
        new(default, default, Optional<RefundPrepaymentErrorResponse>.Some(value), default);

    private static RefundPrepaymentError AsFallback(RawError value) =>
        new(default, default, default, Optional<RawError>.Some(value));

    public bool TryGetRefundPrepaymentBaseErrorsResponse1(out RefundPrepaymentBaseErrorsResponse1 value) =>
        _refundPrepaymentBaseErrorsResponse1Value.TryGetValue(out value);

    public bool TryGetString(out string value) => _stringValue.TryGetValue(out value);

    public bool TryGetRefundPrepaymentErrorResponse(out RefundPrepaymentErrorResponse value) =>
        _refundPrepaymentErrorResponseValue.TryGetValue(out value);

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
