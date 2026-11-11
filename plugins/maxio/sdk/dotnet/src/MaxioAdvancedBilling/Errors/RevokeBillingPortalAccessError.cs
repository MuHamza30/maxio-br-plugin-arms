using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class RevokeBillingPortalAccessError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private RevokeBillingPortalAccessError(Optional<RawError> noContentValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
    }

    private static RevokeBillingPortalAccessError AsNoContent(RawError value) =>
        new(Optional<RawError>.Some(value), default);

    private static RevokeBillingPortalAccessError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    private static Task<RevokeBillingPortalAccessError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.RawBody().As(AsNoContent),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<RevokeBillingPortalAccessError> Response { get; } = new(Create);
}
