namespace MaxioAdvancedBilling.Requests.Entitlements;

/// <summary>
/// The inputs of the ReadSubscriptionEntitlements operation.
/// </summary>
public sealed record ReadSubscriptionEntitlementsRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }
}
