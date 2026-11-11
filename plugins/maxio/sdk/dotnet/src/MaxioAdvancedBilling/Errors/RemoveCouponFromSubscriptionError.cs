using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class RemoveCouponFromSubscriptionError : ApiError
{
    private readonly Optional<SubscriptionRemoveCouponErrors1> _subscriptionRemoveCouponErrors1Value;

    private RemoveCouponFromSubscriptionError(Optional<SubscriptionRemoveCouponErrors1> subscriptionRemoveCouponErrors1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _subscriptionRemoveCouponErrors1Value = subscriptionRemoveCouponErrors1Value;
    }

    private static RemoveCouponFromSubscriptionError AsSubscriptionRemoveCouponErrors1(SubscriptionRemoveCouponErrors1 value) =>
        new(Optional<SubscriptionRemoveCouponErrors1>.Some(value), default);

    private static RemoveCouponFromSubscriptionError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetSubscriptionRemoveCouponErrors1(out SubscriptionRemoveCouponErrors1 value) =>
        _subscriptionRemoveCouponErrors1Value.TryGetValue(out value);

    private static Task<RemoveCouponFromSubscriptionError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<SubscriptionRemoveCouponErrors1>().As(AsSubscriptionRemoveCouponErrors1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<RemoveCouponFromSubscriptionError> Response { get; } = new(Create);
}
