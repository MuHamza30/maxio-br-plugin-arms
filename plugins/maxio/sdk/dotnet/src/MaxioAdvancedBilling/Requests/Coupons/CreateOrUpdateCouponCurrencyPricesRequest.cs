using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Coupons;

/// <summary>
/// The inputs of the CreateOrUpdateCouponCurrencyPrices operation.
/// </summary>
public sealed record CreateOrUpdateCouponCurrencyPricesRequest
{
    /// <summary>
    /// The Advanced Billing id of the coupon
    /// </summary>
    public required int CouponId { get; init; }

    public CouponCurrencyRequest? Body { get; init; }
}
