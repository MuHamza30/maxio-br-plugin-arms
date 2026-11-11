using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ReadBillingPortalLinkError : ApiError
{
    private readonly Optional<ErrorListResponse1> _errorListResponse1Value;

    private readonly Optional<TooManyManagementLinkRequestsError1> _tooManyManagementLinkRequestsError1Value;

    private ReadBillingPortalLinkError(Optional<ErrorListResponse1> errorListResponse1Value,
        Optional<TooManyManagementLinkRequestsError1> tooManyManagementLinkRequestsError1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _errorListResponse1Value = errorListResponse1Value;
        _tooManyManagementLinkRequestsError1Value = tooManyManagementLinkRequestsError1Value;
    }

    private static ReadBillingPortalLinkError AsErrorListResponse1(ErrorListResponse1 value) =>
        new(Optional<ErrorListResponse1>.Some(value), default, default);

    private static ReadBillingPortalLinkError AsTooManyManagementLinkRequestsError1(TooManyManagementLinkRequestsError1 value) =>
        new(default, Optional<TooManyManagementLinkRequestsError1>.Some(value), default);

    private static ReadBillingPortalLinkError AsFallback(RawError value) =>
        new(default, default, Optional<RawError>.Some(value));

    public bool TryGetErrorListResponse1(out ErrorListResponse1 value) =>
        _errorListResponse1Value.TryGetValue(out value);

    public bool TryGetTooManyManagementLinkRequestsError1(out TooManyManagementLinkRequestsError1 value) =>
        _tooManyManagementLinkRequestsError1Value.TryGetValue(out value);

    private static Task<ReadBillingPortalLinkError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<ErrorListResponse1>().As(AsErrorListResponse1),
            429 => response.Json<TooManyManagementLinkRequestsError1>().As(AsTooManyManagementLinkRequestsError1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ReadBillingPortalLinkError> Response { get; } = new(Create);
}
