namespace MaxioAdvancedBilling.Requests.SubscriptionComponents;

/// <summary>
/// The inputs of the DeactivateEventBasedComponent operation.
/// </summary>
public sealed record DeactivateEventBasedComponentRequest
{
    /// <summary>
    /// The Advanced Billing id of the subscription
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// The Advanced Billing id of the component
    /// </summary>
    public required int ComponentId { get; init; }
}
