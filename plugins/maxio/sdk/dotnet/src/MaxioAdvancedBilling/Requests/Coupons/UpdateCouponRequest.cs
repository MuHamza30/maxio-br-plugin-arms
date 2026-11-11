using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Coupons;

/// <summary>
/// The inputs of the UpdateCoupon operation.
/// </summary>
public sealed record UpdateCouponRequest
{
    /// <summary>
    /// The Advanced Billing id of the product family to which the coupon belongs
    /// </summary>
    public required int ProductFamilyId { get; init; }

    /// <summary>
    /// The Advanced Billing id of the coupon
    /// </summary>
    public required int CouponId { get; init; }

    public CouponRequest? Body { get; init; }
}
