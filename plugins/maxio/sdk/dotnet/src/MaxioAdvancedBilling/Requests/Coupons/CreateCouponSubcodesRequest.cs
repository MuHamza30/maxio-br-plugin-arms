using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Coupons;

/// <summary>
/// The inputs of the CreateCouponSubcodes operation.
/// </summary>
public sealed record CreateCouponSubcodesRequest
{
    /// <summary>
    /// The Advanced Billing id of the coupon
    /// </summary>
    public required int CouponId { get; init; }

    public CouponSubcodes? Body { get; init; }
}
