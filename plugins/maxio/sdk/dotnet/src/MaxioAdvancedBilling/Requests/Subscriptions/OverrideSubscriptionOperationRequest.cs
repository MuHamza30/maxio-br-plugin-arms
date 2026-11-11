using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Subscriptions;

/// <summary>
/// The inputs of the OverrideSubscription operation.
/// </summary>
public sealed record OverrideSubscriptionOperationRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// Only these fields are available to be set.
    /// </summary>
    public OverrideSubscriptionRequest? Body { get; init; }
}
