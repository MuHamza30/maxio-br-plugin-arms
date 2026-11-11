using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class RecordPaymentForSubscriptionError : ApiError
{
    private readonly Optional<ErrorListResponse1> _errorListResponse1Value;

    private RecordPaymentForSubscriptionError(Optional<ErrorListResponse1> errorListResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _errorListResponse1Value = errorListResponse1Value;
    }

    private static RecordPaymentForSubscriptionError AsErrorListResponse1(ErrorListResponse1 value) =>
        new(Optional<ErrorListResponse1>.Some(value), default);

    private static RecordPaymentForSubscriptionError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetErrorListResponse1(out ErrorListResponse1 value) =>
        _errorListResponse1Value.TryGetValue(out value);

    private static Task<RecordPaymentForSubscriptionError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<ErrorListResponse1>().As(AsErrorListResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<RecordPaymentForSubscriptionError> Response { get; } = new(Create);
}
