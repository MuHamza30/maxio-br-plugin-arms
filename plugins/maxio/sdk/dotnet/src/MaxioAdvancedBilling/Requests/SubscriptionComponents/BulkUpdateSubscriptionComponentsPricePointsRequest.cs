using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionComponents;

/// <summary>
/// The inputs of the BulkUpdateSubscriptionComponentsPricePoints operation.
/// </summary>
public sealed record BulkUpdateSubscriptionComponentsPricePointsRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    public BulkComponentsPricePointAssignment? Body { get; init; }
}
