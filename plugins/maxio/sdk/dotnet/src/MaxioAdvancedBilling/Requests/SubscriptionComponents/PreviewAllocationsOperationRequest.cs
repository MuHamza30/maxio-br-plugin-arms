using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionComponents;

/// <summary>
/// The inputs of the PreviewAllocations operation.
/// </summary>
public sealed record PreviewAllocationsOperationRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    public PreviewAllocationsRequest? Body { get; init; }
}
