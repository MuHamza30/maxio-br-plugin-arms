namespace MaxioAdvancedBilling.Requests.SubscriptionComponents;

/// <summary>
/// The inputs of the BulkResetSubscriptionComponentsPricePoints operation.
/// </summary>
public sealed record BulkResetSubscriptionComponentsPricePointsRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }
}
