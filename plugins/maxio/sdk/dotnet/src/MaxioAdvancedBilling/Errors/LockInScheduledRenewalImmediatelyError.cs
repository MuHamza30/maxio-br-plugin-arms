using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class LockInScheduledRenewalImmediatelyError : ApiError
{
    private readonly Optional<ErrorListResponse1> _errorListResponse1Value;

    private LockInScheduledRenewalImmediatelyError(Optional<ErrorListResponse1> errorListResponse1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _errorListResponse1Value = errorListResponse1Value;
    }

    private static LockInScheduledRenewalImmediatelyError AsErrorListResponse1(ErrorListResponse1 value) =>
        new(Optional<ErrorListResponse1>.Some(value), default);

    private static LockInScheduledRenewalImmediatelyError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetErrorListResponse1(out ErrorListResponse1 value) =>
        _errorListResponse1Value.TryGetValue(out value);

    private static Task<LockInScheduledRenewalImmediatelyError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<ErrorListResponse1>().As(AsErrorListResponse1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<LockInScheduledRenewalImmediatelyError> Response { get; } = new(Create);
}
