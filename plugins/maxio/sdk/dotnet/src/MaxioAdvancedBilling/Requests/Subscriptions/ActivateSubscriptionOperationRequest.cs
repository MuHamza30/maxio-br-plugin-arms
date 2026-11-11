using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.Subscriptions;

/// <summary>
/// The inputs of the ActivateSubscription operation.
/// </summary>
public sealed record ActivateSubscriptionOperationRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    public ActivateSubscriptionRequest? Body { get; init; }
}
