using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Subscriptions;

/// <summary>
/// The inputs of the ApplyCouponsToSubscription operation.
/// </summary>
public sealed record ApplyCouponsToSubscriptionRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// A code for the coupon that would be applied to a subscription
    /// </summary>
    public string? Code { get; init; }

    public AddCouponsRequest? Body { get; init; }
}
