using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class FindSubscriptionGroupError : ApiError
{
    private readonly Optional<RawError> _noContentValue;

    private FindSubscriptionGroupError(Optional<RawError> noContentValue, Optional<RawError> fallback) : base(fallback)
    {
        _noContentValue = noContentValue;
    }

    private static FindSubscriptionGroupError AsNoContent(RawError value) =>
        new(Optional<RawError>.Some(value), default);

    private static FindSubscriptionGroupError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetNoContent(out RawError value) => _noContentValue.TryGetValue(out value);

    private static Task<FindSubscriptionGroupError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            404 => response.RawBody().As(AsNoContent),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<FindSubscriptionGroupError> Response { get; } = new(Create);
}
