namespace MaxioAdvancedBilling.Requests.SubscriptionComponents;

/// <summary>
/// The inputs of the ReadSubscriptionComponent operation.
/// </summary>
public sealed record ReadSubscriptionComponentRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// The Advanced Billing id of the component. Alternatively, the component's handle prefixed by <c>handle:</c>
    /// </summary>
    public required int ComponentId { get; init; }
}
