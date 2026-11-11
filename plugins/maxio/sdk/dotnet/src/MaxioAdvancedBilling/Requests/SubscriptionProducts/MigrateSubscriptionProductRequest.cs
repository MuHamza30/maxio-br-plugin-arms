using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionProducts;

/// <summary>
/// The inputs of the MigrateSubscriptionProduct operation.
/// </summary>
public sealed record MigrateSubscriptionProductRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    public SubscriptionProductMigrationRequest? Body { get; init; }
}
