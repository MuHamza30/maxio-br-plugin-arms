using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionProducts;

/// <summary>
/// The inputs of the PreviewSubscriptionProductMigration operation.
/// </summary>
public sealed record PreviewSubscriptionProductMigrationRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    public SubscriptionMigrationPreviewRequest? Body { get; init; }
}
