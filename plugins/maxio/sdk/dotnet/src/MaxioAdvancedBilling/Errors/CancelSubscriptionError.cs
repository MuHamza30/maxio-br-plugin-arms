using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models.AnyOf;

namespace MaxioAdvancedBilling.Errors;

public sealed class CancelSubscriptionError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private readonly Optional<CancelSubscriptionErrorResponse> _cancelSubscriptionErrorResponseValue;

    private CancelSubscriptionError(Optional<RawError> noContentValue,
        Optional<CancelSubscriptionErrorResponse> cancelSubscriptionErrorResponseValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
        _cancelSubscriptionErrorResponseValue = cancelSubscriptionErrorResponseValue;
    }

    private static CancelSubscriptionError AsNoContent(RawError value) =>
        new(Optional<RawError>.Some(value), default, default);

    private static CancelSubscriptionError AsCancelSubscriptionErrorResponse(CancelSubscriptionErrorResponse value) =>
        new(default, Optional<CancelSubscriptionErrorResponse>.Some(value), default);

    private static CancelSubscriptionError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    public bool TryGetCancelSubscriptionErrorResponse(out CancelSubscriptionErrorResponse value) =>
        _cancelSubscriptionErrorResponseValue.TryGetValue(out value);

    private static Task<CancelSubscriptionError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            422 => response.Json<CancelSubscriptionErrorResponse>().As(AsCancelSubscriptionErrorResponse),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<CancelSubscriptionError> Response { get; } = new(Create);
}
