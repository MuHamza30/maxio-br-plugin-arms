using MaxioAdvancedBilling.Models;

namespace MaxioAdvancedBilling.Requests.SubscriptionRenewals;

/// <summary>
/// The inputs of the CreateScheduledRenewalConfigurationItem operation.
/// </summary>
public sealed record CreateScheduledRenewalConfigurationItemRequest
{
    /// <summary>
    /// The Chargify id of the subscription.
    /// </summary>
    public required int SubscriptionId { get; init; }

    /// <summary>
    /// The scheduled renewal configuration id.
    /// </summary>
    public required int ScheduledRenewalsConfigurationId { get; init; }

    public ScheduledRenewalConfigurationItemRequest? Body { get; init; }
}
