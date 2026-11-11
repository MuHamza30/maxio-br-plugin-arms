namespace MaxioAdvancedBilling.Requests.Subscriptions;

/// <summary>
/// The inputs of the FindSubscription operation.
/// </summary>
public sealed record FindSubscriptionRequest
{
    /// <summary>
    /// Subscription reference
    /// </summary>
    public string? Reference { get; init; }
}
