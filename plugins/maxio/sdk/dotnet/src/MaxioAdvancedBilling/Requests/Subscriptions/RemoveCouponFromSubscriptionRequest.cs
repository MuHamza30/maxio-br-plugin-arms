namespace MaxioAdvancedBilling.Requests.Subscriptions;

/// <summary>
/// The inputs of the RemoveCouponFromSubscription operation.
/// </summary>
public sealed record RemoveCouponFromSubscriptionRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// The coupon code
    /// </summary>
    public string? CouponCode { get; init; }
}
