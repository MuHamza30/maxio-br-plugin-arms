namespace MaxioAdvancedBilling.Requests.SubscriptionStatus;

/// <summary>
/// The inputs of the RetrySubscription operation.
/// </summary>
public sealed record RetrySubscriptionRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }
}
