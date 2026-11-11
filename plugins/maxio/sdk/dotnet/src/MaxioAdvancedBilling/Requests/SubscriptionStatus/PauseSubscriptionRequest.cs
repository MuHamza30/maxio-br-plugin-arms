using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionStatus;

/// <summary>
/// The inputs of the PauseSubscription operation.
/// </summary>
public sealed record PauseSubscriptionRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    public PauseRequest? Body { get; init; }
}
