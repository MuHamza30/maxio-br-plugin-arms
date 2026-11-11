namespace MaxioAdvancedBilling.Requests.Coupons;

/// <summary>
/// The inputs of the ReadCoupon operation.
/// </summary>
public sealed record ReadCouponRequest
{
    /// <summary>
    /// The Advanced Billing id of the product family to which the coupon belongs
    /// </summary>
    public required int ProductFamilyId { get; init; }

    /// <summary>
    /// The Advanced Billing id of the coupon
    /// </summary>
    public required int CouponId { get; init; }

    /// <summary>
    /// (Optional) If you have defined multiple currencies at the site level, you can pass <c>?currency_prices=true</c> to include an array of currency price data in the response.
    /// </summary>
    public bool? CurrencyPrices { get; init; }
}
