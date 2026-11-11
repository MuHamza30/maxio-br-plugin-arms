using MaxioAdvancedBilling.Models.Enums;

namespace MaxioAdvancedBilling.Requests.SubscriptionRenewals;

/// <summary>
/// The inputs of the ListScheduledRenewalConfigurations operation.
/// </summary>
public sealed record ListScheduledRenewalConfigurationsRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// (Optional) Status filter for scheduled renewal configurations.
    /// </summary>
    public Status? Status { get; init; }
}
