namespace MaxioAdvancedBilling.Requests.Coupons;

/// <summary>
/// The inputs of the ValidateCoupon operation.
/// </summary>
public sealed record ValidateCouponRequest
{
    /// <summary>
    /// The code of the coupon
    /// </summary>
    public required string Code { get; init; }

    /// <summary>
    /// The Advanced Billing id of the product family to which the coupon belongs
    /// </summary>
    public int? ProductFamilyId { get; init; }
}
