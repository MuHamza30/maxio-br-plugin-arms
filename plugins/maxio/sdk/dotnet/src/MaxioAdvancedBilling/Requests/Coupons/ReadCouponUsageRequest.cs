namespace MaxioAdvancedBilling.Requests.Coupons;

/// <summary>
/// The inputs of the ReadCouponUsage operation.
/// </summary>
public sealed record ReadCouponUsageRequest
{
    /// <summary>
    /// The Advanced Billing id of the product family to which the coupon belongs.
    /// </summary>
    public required int ProductFamilyId { get; init; }

    /// <summary>
    /// The Advanced Billing id of the coupon.
    /// </summary>
    public required int CouponId { get; init; }
}
