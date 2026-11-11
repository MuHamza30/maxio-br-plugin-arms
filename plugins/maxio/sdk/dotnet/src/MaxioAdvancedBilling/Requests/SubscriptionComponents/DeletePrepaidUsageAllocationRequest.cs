using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionComponents;

/// <summary>
/// The inputs of the DeletePrepaidUsageAllocation operation.
/// </summary>
public sealed record DeletePrepaidUsageAllocationRequest
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

    public CreditSchemeRequest? Body { get; init; }
}
