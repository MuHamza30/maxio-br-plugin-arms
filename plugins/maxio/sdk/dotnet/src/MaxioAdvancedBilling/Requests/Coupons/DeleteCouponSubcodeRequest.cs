namespace MaxioAdvancedBilling.Requests.Coupons;

/// <summary>
/// The inputs of the DeleteCouponSubcode operation.
/// </summary>
public sealed record DeleteCouponSubcodeRequest
{
    /// <summary>
    /// The Advanced Billing id of the coupon to which the subcode belongs
    /// </summary>
    public required int CouponId { get; init; }

    /// <summary>
    /// The subcode of the coupon
    /// </summary>
    public required string Subcode { get; init; }
}
