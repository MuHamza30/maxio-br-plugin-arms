using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionStatus;

/// <summary>
/// The inputs of the InitiateDelayedCancellation operation.
/// </summary>
public sealed record InitiateDelayedCancellationRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    public CancellationRequest? Body { get; init; }
}
