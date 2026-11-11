using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionComponents;

/// <summary>
/// The inputs of the UpdatePrepaidUsageAllocationExpirationDate operation.
/// </summary>
public sealed record UpdatePrepaidUsageAllocationExpirationDateRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// The Advanced Billing id of the component
    /// </summary>
    public required int ComponentId { get; init; }

    /// <summary>
    /// The Advanced Billing id of the allocation
    /// </summary>
    public required int AllocationId { get; init; }

    public UpdateAllocationExpirationDate? Body { get; init; }
}
