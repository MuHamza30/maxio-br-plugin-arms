using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionComponents;

/// <summary>
/// The inputs of the AllocateComponent operation.
/// </summary>
public sealed record AllocateComponentRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// The Advanced Billing id of the component
    /// </summary>
    public required int ComponentId { get; init; }

    public CreateAllocationRequest? Body { get; init; }
}
