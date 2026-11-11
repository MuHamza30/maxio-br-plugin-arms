using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class PurgeSubscriptionError : ApiError
{
    private readonly Optional<SubscriptionResponse> _subscriptionResponseValue;

    private PurgeSubscriptionError(Optional<SubscriptionResponse> subscriptionResponseValue,
        Optional<RawError> fallback) : base(fallback)
    {
        _subscriptionResponseValue = subscriptionResponseValue;
    }

    private static PurgeSubscriptionError AsSubscriptionResponse(SubscriptionResponse value) =>
        new(Optional<SubscriptionResponse>.Some(value), default);

    private static PurgeSubscriptionError AsFallback(RawError value) => new(default, Optional<RawError>.Some(value));

    public bool TryGetSubscriptionResponse(out SubscriptionResponse value) =>
        _subscriptionResponseValue.TryGetValue(out value);

    private static Task<PurgeSubscriptionError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            400 => response.Json<SubscriptionResponse>().As(AsSubscriptionResponse),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<PurgeSubscriptionError> Response { get; } = new(Create);
}
