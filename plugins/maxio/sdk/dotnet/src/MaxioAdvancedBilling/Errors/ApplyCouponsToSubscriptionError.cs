using System.Threading.Tasks;
using MaxioAdvancedBilling.Core.ErrorResponse;
using MaxioAdvancedBilling.Core.Models;
using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Errors;

public sealed class ApplyCouponsToSubscriptionError : ApiError
{
    private readonly Optional<SubscriptionAddCouponError1> _subscriptionAddCouponError1Value;

    private ApplyCouponsToSubscriptionError(Optional<SubscriptionAddCouponError1> subscriptionAddCouponError1Value,
        Optional<RawError> fallback) : base(fallback)
    {
        _subscriptionAddCouponError1Value = subscriptionAddCouponError1Value;
    }

    private static ApplyCouponsToSubscriptionError AsSubscriptionAddCouponError1(SubscriptionAddCouponError1 value) =>
        new(Optional<SubscriptionAddCouponError1>.Some(value), default);

    private static ApplyCouponsToSubscriptionError AsFallback(RawError value) =>
        new(default, Optional<RawError>.Some(value));

    public bool TryGetSubscriptionAddCouponError1(out SubscriptionAddCouponError1 value) =>
        _subscriptionAddCouponError1Value.TryGetValue(out value);

    private static Task<ApplyCouponsToSubscriptionError> Create(FailedResponse response) =>
        response.StatusCode switch
        {
            422 => response.Json<SubscriptionAddCouponError1>().As(AsSubscriptionAddCouponError1),
            _ => response.RawBody().As(AsFallback)
        };

    internal static ApiErrorResponse<ApplyCouponsToSubscriptionError> Response { get; } = new(Create);
}
